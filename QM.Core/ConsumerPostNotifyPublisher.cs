using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QM.Core.Abstractions;
using QM.Core.Abstractions.Enums;
using QM.Core.Common;
using QM.Core.Helper;
using QM.Models.Abstractions;
using QM.Models.OutputModels;
using System.Text;

namespace QM.Core
{
    public partial class ConsumerPostNotifyPublisher<TAppContext, TInputModel> : IConsumerPostNotifyPublisher<TAppContext, TInputModel> where TInputModel : IRegistrationModel
    {
        public class ExecutedPostPoolItem
        {
            public string Url { get; set; }
            public HttpContent HttpContent { get; set; }
            public Guid Guid { get; set; }
            public bool IsSuccess = false;
            public int RetryCount = 0;

            private const int MaxRetries = 3;
            private int _maxRetries;

            public ExecutedPostPoolItem(int maxRetries = MaxRetries)
            {
                this._maxRetries = maxRetries;
            }
            public bool GetIsRetryAllowed()
            {
                return this.RetryCount <= _maxRetries;
            }

        }

        private List<ExecutedPostPoolItem> _executedPostPoolList;
        private readonly ILogger<TAppContext> _logger;
        private readonly ParameterHelper _parameterHelper = new();

        public ConsumerPostNotifyPublisher(
           ILogger<TAppContext> logger
           )
        {
            this._logger = logger;
            this._executedPostPoolList = new List<ExecutedPostPoolItem>();
        }


        public async Task ExecutePostNotifications(
            TInputModel inputModel,
            DateTime perstistedDateTimeAt,
            DateTime persistedDateTimeEnd,
            List<PersistStrategyType> persistStrategyTypes
            )
        {

            var outputModel = new OutputModel<TInputModel>(
                model: inputModel,
                persistedDateTimeAt: perstistedDateTimeAt,
                persistedDateTimeEnd: persistedDateTimeEnd,
                persistStrategyTypes: persistStrategyTypes
                );

            var content = new StringContent(JsonConvert.SerializeObject(outputModel), Encoding.UTF8, "application/json");

            var tasks = new List<Task<string>>();
            foreach (var parameterType in new[] {
                ParameterType.urlA,
                ParameterType.urlB})
            {
                tasks.Add(this.ExecutePostAsync(parameterType, content, inputModel.Guid));
            }
            var responses = await Task.WhenAll(tasks);
            foreach (var response in responses)
            {
                this._logger.LogInformation($"{inputModel.Guid} Executed post notification response : {response}");
            }

            await this.ExecuteRetryStrategy();
        }

        private async Task<string> ExecutePostAsync(ParameterType parameterType, HttpContent httpContent, Guid guid)
        {
            var url = this._parameterHelper.GetParameter(parameterType);
            var httpPostMaxRetries = this._parameterHelper.GetParameterInt(ParameterType.HttpPostMaxRetries);
            try
            {                
                var httpResponse = await HttpHelper.PostAsync(url, httpContent);
                _executedPostPoolList.Add(new ExecutedPostPoolItem(maxRetries: httpPostMaxRetries) { IsSuccess = true, Guid = guid,  Url = url, HttpContent = httpContent });
                return httpResponse;
            }
            catch (Exception ex)
            {                
                _executedPostPoolList.Add(new ExecutedPostPoolItem(maxRetries: httpPostMaxRetries) { IsSuccess = false, Guid = guid, Url = url, HttpContent = httpContent });
                var message = $"{guid} Failed to execute post Async {url} {httpContent}: {ex.Message}";
                this._logger.LogError(message);
                return message;
            }
        }
              

    }
}

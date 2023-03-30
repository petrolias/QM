using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QM.Core.Abstractions.Enums;
using QM.Core.Common;
using QM.Models.Abstractions;
using QM.Models.OutputModels;
using System.Text;

namespace QM.Core
{
    public partial class ConsumerPersistExecutor<TAppContext, TInputModel>
        where TInputModel : IRegistrationModel
    {
        private List<(ParameterType, bool)> _executedPostPoolList;

        private async Task ExecutePostNotifications(
            TInputModel inputModel,             
            DateTime  perstistedDateTimeAt, 
            DateTime persistedDateTimeEnd,
            List<PersistStrategyType> persistStrategyTypes
            ) {

            var outputModel = new OutputModel<TInputModel>(
                model: inputModel,                
                persistedDateTimeAt: perstistedDateTimeAt,
                persistedDateTimeEnd: persistedDateTimeEnd,
                persistStrategyTypes: persistStrategyTypes
                );

            var content = new StringContent(JsonConvert.SerializeObject(outputModel), Encoding.UTF8, "application/json");
            
            var tasks = new List<Task<string>>();
            foreach (var parameterType in new [] { 
                ParameterType.urlA, 
                ParameterType.urlB})
            {
                tasks.Add(this.ExecutePostAsync(parameterType, content));                
            }
            var responses = await Task.WhenAll(tasks);
            foreach (var response in responses)
            {
                this._logger.LogInformation($"{this.GetGuid()} Executed post notification response : {response}");
            }
        }

        private async Task<string> ExecutePostAsync(ParameterType parameterType, HttpContent httpContent)
        {
            try
            {
                var httpResponse = await HttpHelper.PostAsync(this.parameterHelper.GetParameter(parameterType), httpContent);
                _executedPostPoolList.Add((parameterType, true));
                return httpResponse;
            }
            catch (Exception e)
            {
                // Record the failure
                var message = $"{this.GetGuid()} Failed to execute post Async {parameterType} {httpContent}: {e.Message}";
                _executedPostPoolList.Add((parameterType, false));
                this._logger.LogError(message);
                return message;
            }
        }
       
    }
}

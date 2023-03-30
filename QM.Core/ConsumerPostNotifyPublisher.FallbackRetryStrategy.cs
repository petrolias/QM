using Microsoft.Extensions.Logging;
using QM.Core.Abstractions.Enums;
using QM.Core.Common;
using QM.Models.Abstractions;
namespace QM.Core
{
    public partial class ConsumerPostNotifyPublisher<TAppContext, TInputModel>
        where TInputModel : IRegistrationModel
    {
              
        private bool HasUnexecutedItem()
        {
            return this._executedPostPoolList.Any(x => x.IsSuccess == false);
        }       
       
        /// <summary>
        /// executes the retry strategy
        /// </summary>
        private async Task ExecuteRetryStrategy()
        {
            //Add retry logic here if needed
            if (!this.HasUnexecutedItem()) { return; }
            while (this._executedPostPoolList.Any())
            {
                foreach (var item in this._executedPostPoolList.ToArray())
                {
                    if (item.IsSuccess || !item.GetIsRetryAllowed())
                    {
                        this._executedPostPoolList.Remove(item);
                        continue;
                    }
                    item.RetryCount++;
                    var result = await DoAsyncTask(item);
                    if (!result)
                    {
                        this._executedPostPoolList.Remove(item);
                    }

                }
            }

        }

        private async Task<bool> DoAsyncTask(ExecutedPostPoolItem executedPostPoolItem)
        {            
            this._logger.LogInformation($"Starting task for item {executedPostPoolItem.Guid} and url {executedPostPoolItem.Url} retry count {executedPostPoolItem.RetryCount}");
            //await Task.Delay(1000);
            try
            {
                var httpResponse = await HttpHelper.PostAsync(executedPostPoolItem.Url, executedPostPoolItem.HttpContent);
                this._logger.LogInformation($"{executedPostPoolItem.Guid} Executed post notification response : {httpResponse}");
            }
            catch (Exception ex) {                              
                this._logger.LogError($"{executedPostPoolItem.Guid} Failed to execute post Async {executedPostPoolItem.HttpContent}: {ex.Message}");
                return false;

            }
            return true;
        }
    }
}
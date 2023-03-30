using QM.Core.Abstractions.Enums;
using QM.Models.Abstractions;
namespace QM.Core
{
    public partial class ConsumerPersistExecutor<TAppContext, TInputModel>
        where TInputModel : IRegistrationModel
    {

        private bool HasUnexecutedItem()
        {
            return this._executedPersistStrategyPoolList.Any(x => x.Item2 == false);
        }

        private List<PersistStrategyType> GetNonExecutedPersistStrategyTypes()
        {
            return this._executedPersistStrategyPoolList.Where(x => x.Item2 == false).Select(x => x.Item1).ToList();
        }

        /// <summary>
        /// executes the retry strategy if enabled and has unexecuted items
        /// </summary>
        private async Task ExecuteRetryStrategy()
        {
            //Add retry logic here if needed
            if (this.HasUnexecutedItem())
            {          
                await this.ConsumeAndPersistAsync(this.GetNonExecutedPersistStrategyTypes(), this._inputModel);
            }
        }
    }
}

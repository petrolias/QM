using QM.Models.Abstractions;
namespace QM.Core
{
    public partial class CommandExecutor<TAppContext, TInputModel>
        where TInputModel : IRegistrationModel
    {

        private bool HasUnexecutedItem()
        {
            return this._executedList.Any(x => x.Item2 == false);
        }

        /// <summary>
        /// executes the retry strategy if enabled and has unexecuted items
        /// </summary>
        private async Task ExecuteRetryStrategy()
        {
            //Add retry logic here if needed
            if (isRetryStrategyEnabled &&
                this.HasUnexecutedItem())
            {
                var executor = new CommandExecutor<TAppContext, TInputModel>(
                    this._logger, this._repository,
                    _toExecuteList, this._inputModel);
                await executor.ExecuteCommandsAsync();
            }
        }
    }
}

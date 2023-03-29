using Microsoft.Extensions.Logging;
using QM.Core.Abstractions.Enums;
using QM.Models.Abstractions;

namespace QM.Core
{
    public class CommandExecutor<TAppContext, TInputModel>         
        where TInputModel : IRegistrationModel
    {

        private const bool IsRetryStrategyEnabled = false;
        private List<PersistSystemType> toExecuteList;
        private List<(PersistSystemType, bool)> executedList;
        private DateTime executionDateTime;
        private TInputModel _inputModel;
        private readonly ILogger<TAppContext> _logger;
        
        private bool HasUnexecutedItem() {
            return this.toExecuteList.Count > 0;
        }
      
        public CommandExecutor(ILogger<TAppContext> logger, List<PersistSystemType> toExecuteList, TInputModel inputModel)
        {
            this.toExecuteList = toExecuteList;
            this.executedList = new List<(PersistSystemType, bool)>();
            this.executionDateTime = DateTime.UtcNow;
            this._inputModel = inputModel;
            this._logger = logger;
        }

        public async Task ExecuteCommandsAsync()
        {
            this._logger.LogInformation("Executing Commands Async");
            var tasks = new List<Task>();
            foreach (var command in toExecuteList)
            {
                tasks.Add(ExecuteCommandAsync(command));
            }
            await Task.WhenAll(tasks);

            // Verify for success
            foreach (var (command, success) in executedList)
            {
                if (success) {
                    toExecuteList.Remove(command);
                }                    
            }

            //Add retry logic here if enabled
            this.ExecuteRetryStrategy();          
        }        

        private async Task ExecuteCommandAsync(PersistSystemType persistSystemType)
        {
            try
            {
                // Execute the command here and record the execution time
                DateTime startTime = DateTime.UtcNow;
                this._logger.LogInformation($"Executing {persistSystemType}...");
                await Task.Delay(1000); // Simulate command execution time
                DateTime endTime = DateTime.UtcNow;
                this._logger.LogInformation($"Executed {persistSystemType} successfully in {(endTime - startTime).TotalMilliseconds} ms.");
                executedList.Add((persistSystemType, true));
            }
            catch (Exception e)
            {
                // Record the failure time and log the exception message
                this._logger.LogError($"Failed to execute {persistSystemType}: {e.Message}");
                executedList.Add((persistSystemType, false));
            }
        }
   
        /// <summary>
        /// executes the retry strategy if enabled and has unexecuted items
        /// </summary>
        private async void ExecuteRetryStrategy()
        {
            //Add retry logic here if needed
            if (IsRetryStrategyEnabled &&
                this.HasUnexecutedItem())
            {
                var executor = new CommandExecutor<TAppContext, TInputModel>(this._logger, toExecuteList, this._inputModel);
                await executor.ExecuteCommandsAsync();
            }            
        }
    }
}

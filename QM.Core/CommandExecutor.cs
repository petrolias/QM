using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QM.Core.Abstractions.Enums;
using QM.Core.Common;
using QM.Core.Helper;
using QM.DAL.Abstractions;
using QM.Models.Abstractions;
using QM.Models.OutputModels;
using System.Text;

namespace QM.Core
{
    public partial class CommandExecutor<TAppContext, TInputModel>
        where TInputModel : IRegistrationModel
    {

        private readonly bool _isRetryStrategyEnabled;
        private List<PersistStrategyType> _toExecuteList;
        private List<(PersistStrategyType, bool)> _executedList;
        private TInputModel _inputModel;
        private readonly ILogger<TAppContext> _logger;
        private readonly IRepository _repository;
        private readonly ParameterHelper parameterHelper = new();
        public static Guid _guid = Guid.NewGuid();

        //Returns executed persist system types
        private List<PersistStrategyType> GetExecutedPersistStrategyTypes()
        {
            return this._executedList.Where(x => x.Item2 == true).Select(x => x.Item1).ToList();
        }

        

        public CommandExecutor(
            ILogger<TAppContext> logger,
            IRepository repository,
            List<PersistStrategyType> toExecuteList, 
            TInputModel inputModel,
            bool isRetryStrategyEnabled = false
            )
        {
            this._logger = logger;
            this._repository = repository;

            this._toExecuteList = toExecuteList;
            this._executedList = new List<(PersistStrategyType, bool)>();
            this._inputModel = inputModel;
            this._isRetryStrategyEnabled = isRetryStrategyEnabled;

        }

        public async Task ExecuteCommandsAsync()
        {
            var executeCommandsStart = DateTime.UtcNow;
            this._logger.LogInformation($"{_guid} Executing Commands Async at {executeCommandsStart}");

            //execute the tasks
            var tasks = new List<Task>();
            foreach (var command in _toExecuteList)
            {
                tasks.Add(this.ExecuteCommandAsync(command));
            }
            await Task.WhenAll(tasks);
            var executeCommandsEnd = DateTime.UtcNow;
            //get only successfull calls
            await ExecutePostNotifications(this._inputModel, executeCommandsStart, executeCommandsEnd, this.GetExecutedPersistStrategyTypes());
            this._logger.LogInformation($"{_guid} Executing Commands Completed at {executeCommandsEnd}");

            //Add retry logic here if enabled
            await this.ExecuteRetryStrategy();
        }

        private async Task ExecuteCommandAsync(PersistStrategyType persistStrategyType)
        {
            try
            {
                await ExecutePersistStrategy(persistStrategyType, this._inputModel);
                _executedList.Add((persistStrategyType, true));
            }
            catch (Exception e)
            {
                // Record the failure time and log the exception message
                this._logger.LogError($"{_guid} Failed to execute {persistStrategyType}: {e.Message}");
                _executedList.Add((persistStrategyType, false));
            }
        }      

        //selects and executes the appropriate strategy to be used based on perists system type
        private async Task ExecutePersistStrategy(PersistStrategyType persistStrategyType, TInputModel inputModel)
        {
            this._logger.LogInformation($"Executing {persistStrategyType}...");
            switch (persistStrategyType)
            {
                case PersistStrategyType.File:
                    await new FileIO().AppendToFileAsync(inputModel);
                    break;
                case PersistStrategyType.Db:
                    await this._repository.SaveChangesAsync(inputModel);
                    break;
                default:
                    break;
            }

        }
                 
    }
}

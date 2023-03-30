using Microsoft.Extensions.Logging;
using QM.Core.Abstractions;
using QM.Core.Abstractions.Enums;
using QM.Core.Helper;
using QM.DAL.Abstractions;
using QM.DAL.Mapper;
using QM.Models.Abstractions;

namespace QM.Core
{
    public partial class ConsumerPersistExecutor<TAppContext, TInputModel> : IConsumerPersistExecutor<TAppContext, TInputModel> where TInputModel : IRegistrationModel
    {        
        private List<PersistStrategyType> _toExecuteList;
        private List<(PersistStrategyType, bool)> _executedPersistStrategyPoolList;
        private TInputModel _inputModel;
        private readonly ILogger<TAppContext> _logger;
        private readonly IRepository _repository;
        private readonly ParameterHelper parameterHelper = new();
        public static Guid _guid = Guid.NewGuid();

        //Returns executed persist system types
        private List<PersistStrategyType> GetExecutedPersistStrategyTypes()
        {
            return this._executedPersistStrategyPoolList.Where(x => x.Item2 == true).Select(x => x.Item1).ToList();
        }


        public ConsumerPersistExecutor(
            ILogger<TAppContext> logger,
            IRepository repository             
            )
        {
            this._logger = logger;
            this._repository = repository;           
            this._executedPersistStrategyPoolList = new List<(PersistStrategyType, bool)>();            
        }

        public async Task ConsumeAndPersistAsync(
            List<PersistStrategyType> toExecuteList,
            TInputModel inputModel)
        {
            this._toExecuteList = toExecuteList;
            this._inputModel = inputModel;
            var executeCommandsStart = DateTime.UtcNow;
            this._logger.LogInformation($"{_guid} Executing Commands Async at {executeCommandsStart}");

            //execute the tasks
            var tasks = new List<Task>();
            foreach (var command in _toExecuteList)
            {
                tasks.Add(this.ExecutePeristStrategyWrapperAsync(command));
            }
            await Task.WhenAll(tasks);
            var executeCommandsEnd = DateTime.UtcNow;
            //get only successfull calls
            await ExecutePostNotifications(this._inputModel, executeCommandsStart, executeCommandsEnd, this.GetExecutedPersistStrategyTypes());
            this._logger.LogInformation($"{_guid} Executing Commands Completed at {executeCommandsEnd}");

            //Add retry logic here if enabled
            //await this.ExecuteRetryStrategy();
        }

        private async Task ExecutePeristStrategyWrapperAsync(PersistStrategyType persistStrategyType)
        {
            try
            {                
                await ExecutePersistStrategySelector(persistStrategyType, this._inputModel.GetDBModel());
                _executedPersistStrategyPoolList.Add((persistStrategyType, true));
            }
            catch (Exception e)
            {
                // Record the failure time and log the exception message
                this._logger.LogError($"{_guid} Failed to execute {persistStrategyType}: {e.Message}");
                _executedPersistStrategyPoolList.Add((persistStrategyType, false));
            }
        }

        //selects and executes the appropriate strategy to be used based on perists system type
        private async Task ExecutePersistStrategySelector(PersistStrategyType persistStrategyType, IRegistrationModel inputModel)
        {
            this._logger.LogInformation($"Executing {persistStrategyType}...");
            switch (persistStrategyType)
            {
                case PersistStrategyType.File:
                    await new FileIO().AddFile(inputModel);
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

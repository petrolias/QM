using Microsoft.Extensions.Logging;
using QM.Core.Abstractions;
using QM.Core.Abstractions.Enums;
using QM.Core.Helper;
using QM.Mapper.Abstractions;
using QM.Mapper;
using QM.Models.Abstractions;
using QM.Models.Validations.Extensions;
namespace QM.Core
{
    public partial class ConsumerPersistExecutor<TAppContext, TInputModel> : IConsumerPersistExecutor<TAppContext, TInputModel> where TInputModel : IRegistrationModel
    {
        private List<PersistStrategyType> _toExecutePersistStrategyTypes;
        private List<(PersistStrategyType, bool)> _executedPersistStrategyPoolList;
        private TInputModel _inputModel;
        private readonly ILogger<TAppContext> _logger;
        private readonly IRepository _repository;
        private readonly IConsumerPostNotifyPublisher<TAppContext, TInputModel> _consumerPostNotifyPublisher;
        
        private Guid GetGuid(){ return this._inputModel.Guid; }

        //Returns executed persist system types
        private List<PersistStrategyType> GetExecutedPersistStrategyTypes()
        {
            return this._executedPersistStrategyPoolList.Where(x => x.Item2 == true).Select(x => x.Item1).ToList();
        }
     
        public ConsumerPersistExecutor(
            ILogger<TAppContext> logger,
            IRepository repository,  
            IConsumerPostNotifyPublisher<TAppContext, TInputModel> consumerPostNotifyPublisher
            )
        {
            this._logger = logger;
            this._repository = repository;           
            this._consumerPostNotifyPublisher = consumerPostNotifyPublisher;
            this._executedPersistStrategyPoolList = new List<(PersistStrategyType, bool)>();            
        }

        /// <summary>
        /// Execute 
        /// </summary>
        /// <param name="toExecuteList"></param>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        public async Task ConsumeAndPersistAsync(
            List<PersistStrategyType> toExecutePersistStrategyTypes,
            TInputModel inputModel)
        {
            var executeCommandsStart = DateTime.UtcNow;

            this._toExecutePersistStrategyTypes = toExecutePersistStrategyTypes;
            this._inputModel = inputModel;
            if (!_inputModel.GetValidation().IsSuccess) {
                Task.FromException(_inputModel.GetValidation().Exception);                
            }
            
            this._logger.LogInformation($"{this.GetGuid()} Executing Commands Async at {executeCommandsStart}");

            //execute the tasks
            var tasks = new List<Task>();
            foreach (var command in _toExecutePersistStrategyTypes)
            {
                tasks.Add(this.ExecutePeristStrategyWrapperAsync(command));
            }
            await Task.WhenAll(tasks);
            var executeCommandsEnd = DateTime.UtcNow;
            //get only successfull calls
            await this._consumerPostNotifyPublisher.ExecutePostNotifications(this._inputModel, 
                executeCommandsStart,
                executeCommandsEnd, 
                this.GetExecutedPersistStrategyTypes());
            this._logger.LogInformation($"{this.GetGuid()} Executing Commands Completed at {executeCommandsEnd}");                     
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
                this._logger.LogError($"{this.GetGuid()} Failed to execute {persistStrategyType}: {e.Message}");
                _executedPersistStrategyPoolList.Add((persistStrategyType, false));
            }
        }

        //selects and executes the appropriate strategy to be used based on perists system type
        private async Task ExecutePersistStrategySelector(PersistStrategyType persistStrategyType, IRegistrationModel inputModel)
        {
            this._logger.LogInformation($"{this.GetGuid()} Executing {persistStrategyType}...");
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

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
    public class CommandExecutor<TAppContext, TInputModel>
        where TInputModel : IRegistrationModel
    {

        private const bool IsRetryStrategyEnabled = false;
        private List<PersistStrategyType> toExecuteList;
        private List<(PersistStrategyType, bool)> executedList;
        private TInputModel _inputModel;
        private readonly ILogger<TAppContext> _logger;
        private readonly IRepository _repository;
        private readonly ParameterHelper parameterHelper = new();
        public static Guid _guid = Guid.NewGuid();

        //Returns executed persist system types
        private List<PersistStrategyType> GetExecutedPersistStrategyTypes()
        {
            return this.executedList.Where(x => x.Item2 == true).Select(x => x.Item1).ToList();
        }

        private bool HasUnexecutedItem()
        {
            return this.executedList.Any(x => x.Item2 == false);
        }

        public CommandExecutor(
            ILogger<TAppContext> logger,
            IRepository repository,
            List<PersistStrategyType> toExecuteList, TInputModel inputModel)
        {
            this._logger = logger;
            this._repository = repository;

            this.toExecuteList = toExecuteList;
            this.executedList = new List<(PersistStrategyType, bool)>();
            this._inputModel = inputModel;
        }

        public async Task ExecuteCommandsAsync()
        {
            var executeCommandsStart = DateTime.UtcNow;
            this._logger.LogInformation($"{_guid} Executing Commands Async at {executeCommandsStart}");

            //execute the tasks
            var tasks = new List<Task>();
            foreach (var command in toExecuteList)
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
                executedList.Add((persistStrategyType, true));
            }
            catch (Exception e)
            {
                // Record the failure time and log the exception message
                this._logger.LogError($"{_guid} Failed to execute {persistStrategyType}: {e.Message}");
                executedList.Add((persistStrategyType, false));
            }
        }

        /// <summary>
        /// executes the retry strategy if enabled and has unexecuted items
        /// </summary>
        private async Task ExecuteRetryStrategy()
        {
            //Add retry logic here if needed
            if (IsRetryStrategyEnabled &&
                this.HasUnexecutedItem())
            {
                var executor = new CommandExecutor<TAppContext, TInputModel>(
                    this._logger, this._repository,
                    toExecuteList, this._inputModel);
                await executor.ExecuteCommandsAsync();
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
                this._logger.LogInformation($"{_guid} Executed post notification response : {response}");
            }
        }

        private async Task<string> ExecutePostAsync(ParameterType parameterType, HttpContent httpContent)
        {
            try
            {
                var httpResponse = await HttpHelper.PostAsync(this.parameterHelper.GetParameter(parameterType), httpContent);
                return httpResponse;
            }
            catch (Exception e)
            {
                // Record the failure
                var message = $"{_guid} Failed to execute post Async {parameterType} {httpContent}: {e.Message}";
                this._logger.LogError(message);
                return message;
            }
        }
    }
}

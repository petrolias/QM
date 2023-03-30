using Microsoft.Extensions.DependencyInjection;
using QM.Mapper.Abstractions;
using QM.Core.Extensions;
using Microsoft.Extensions.Logging;
using QM.Models.DomainModels;
using QM.Core.Abstractions;
using QA.External.Models;
using QM.Mapper;

namespace QA.ConsoleApp
{
    public class Playground<TAppContext>
    {
        private IServiceProvider _serviceProvider { get; set; }
        private ILogger<TAppContext> _logger { get => this._serviceProvider.GetLogger<TAppContext>(); }

        private IConsumerPersistExecutor<TAppContext, RegistrationModel> _consumerPersistExecutor { get => this._serviceProvider.GetConsumerPersistExecutor<TAppContext, RegistrationModel>(); }


        public Playground()
        {
            this._serviceProvider = new ServiceCollection()
                .AddDbRepository()
                .AddLoggerService()
                .AddConsumerPostNotifyPublisher<TAppContext, RegistrationModel>()
                .AddConsumerPersistExecutor<TAppContext, RegistrationModel>()
                .BuildServiceProvider();
        }
        public async Task<bool> ExecuteTaskAsync(InputRegistrationModel inputRegistrationModel)
        {            
            this._logger.LogInformation("Executing Test task async");
            try
            {               
                var registrationModel = inputRegistrationModel.GetDomainModel();
                await this._consumerPersistExecutor.ConsumeAndPersistAsync(
                    ExecutionStrategy.DefaultPersistStrategyTypesStrategies,
                    registrationModel);
            }
            catch (Exception ex) {
                this._logger.LogInformation($"Executing Test task async failed {ex.Message}");
                return false;

            }
            return true;        
        }
    }
}

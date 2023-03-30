using Microsoft.Extensions.DependencyInjection;
using QM.DAL.Abstractions;
using QM.Core.Extensions;
using Microsoft.Extensions.Logging;
using QM.Models.InputModels;
using QM.Core.Abstractions;

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
                .AddConsumerPersistExecutor<TAppContext, RegistrationModel>()
                .BuildServiceProvider();
        }
        public async Task ExecuteTaskAsync()
        {            
            this._logger.LogInformation("Executing Test task async");

            var registrationModel = new RegistrationModel() { UserId = 1, UserName = "TestUsername" };
            await this._consumerPersistExecutor.ConsumeAndPersistAsync(
                ExecutionStrategy.DefaultPersistStrategyTypesStragegies,
                registrationModel);
        }
    }
}

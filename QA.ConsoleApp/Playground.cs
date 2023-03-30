using Microsoft.Extensions.DependencyInjection;
using QM.DAL.Abstractions;
using QM.DAL.Models;
using QM.Core.Abstractions.Enums;
using QM.Core.Extensions;
using Microsoft.Extensions.Logging;
using QM.Core;
using QM.Models.InputModels;
using QM.Core.Abstractions;

namespace QA.ConsoleApp
{
    public class Playground<TAppContext>
    {
        private IServiceProvider _serviceProvider { get; set; }
        private IRepository _repository { get => this._serviceProvider.GetRepository(); }
        private ILogger<TAppContext> _logger { get => this._serviceProvider.GetLogger<TAppContext>(); }

        public Playground()
        {
            this._serviceProvider = new ServiceCollection()
                .AddDbRepository()
                .AddLoggerService()
                .BuildServiceProvider();
        }
        public async Task ExecuteTaskAsync()
        {            
            this._logger.LogInformation("Executing Test task async");
 
            var registrationModel = new RegistrationModel() { UserId = 1, UserName = "TestUsername" };
            var executor = new CommandExecutor<TAppContext, RegistrationModel>(
                this._logger, 
                this._repository,
                ExecutionStrategy.DefaultPersistStrategyTypesStragegy, 
                registrationModel);

            await executor.ExecuteCommandsAsync();
        }
    }
}

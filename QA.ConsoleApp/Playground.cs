using Microsoft.Extensions.DependencyInjection;
using QM.DAL.Abstractions;
using QM.DAL.Models;
using QM.Core.Abstractions.Enums;
using QM.Core.Extensions;
using Microsoft.Extensions.Logging;
using QM.Core;

namespace QA.ConsoleApp
{
    public class Playground
    {
        private IServiceProvider _serviceProvider { get; set; }
        private IRepository _repository { get => this._serviceProvider.GetRepository(); }
        private ILogger<Program> _logger { get => this._serviceProvider.GetLogger<Program>(); }

        public Playground()
        {
            this._serviceProvider = new ServiceCollection()
                .AddDbRepository()
                .AddLoggerService()
                .BuildServiceProvider();
        }
        public async Task TestTaskAsync()
        {
            this._logger.LogInformation("Test");
            var toExecuteList = new List<PersistSystemType>() {
                PersistSystemType.File,
                PersistSystemType.Db
            };
            var executor = new CommandExecutor(toExecuteList, "Test message");

            await executor.ExecuteCommandsAsync();
        }
    }
}

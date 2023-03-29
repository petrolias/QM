using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QM.DAL;
using QM.DAL.Abstractions;
using QM.DAL.Models;
using QM.Core.Abstractions.Enums;
using QM.Core;
using Microsoft.Extensions.Logging;
using Serilog.Context;
namespace QM.TestProject
{

    public class PlayGround
    {
        private const string InMemoryDataBaseName = "InMemoryDatabase";        
        private IServiceProvider _serviceProvider { get; set; }
        private IRepository _repository { get => this._services.GetRepository(); }

        private readonly ILogger<PlayGround> _logger;
        public PlayGround()
        {                                    
            this._serviceProvider = new ServiceCollection()
                .AddDbRepository()
                .AddLoggerService()
                .BuildServiceProvider();

        }
        [Fact]
        public void TestRepositorySaveChangesAsync()
        {
            var registrationModel = new RegistrationModelDB();            
            this._repository.SaveChangesAsync(registrationModel);
            // Assert that the entity was added successfully
            Assert.NotEqual(0, registrationModel.Id);            
        }

        [Fact]
        public async void TestTaskAsync()
        {
            using (var scope = this._services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetService<ILogger<MyClass>>();

                // Act
                using (LogContext.PushProperty("SomeProperty", "SomeValue"))
                {
                    logger.LogInformation("This is a test log");
                }

                // Assert
                // ...
            }

            using (LogContext.PushProperty("Guid", Guid.NewGuid()))
            {
                _logger.LogInformation("This is a scoped log with Guid %K", "Guid");
            }
            var toExecuteList = new List<PersistSystemType>() {
                PersistSystemType.File,
                PersistSystemType.Db                
            };                        
            var executor = new CommandExecutor(toExecuteList, "Test message");

            await executor.ExecuteCommandsAsync();
        }

    }
}
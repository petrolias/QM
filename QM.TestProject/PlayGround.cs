
using Microsoft.Extensions.DependencyInjection;
using QM.DAL.Abstractions;
using QM.DAL.Models;
using QM.Core.Abstractions.Enums;
using QM.Core.Extensions;
using Microsoft.Extensions.Logging;
using QM.Core;

namespace QM.TestProject
{

    public class PlayGround
    {          
        private IServiceProvider _serviceProvider { get; set; }
        private IRepository _repository { get => this._serviceProvider.GetRepository(); }
        private ILogger<PlayGround> _logger { get => this._serviceProvider.GetLogger<PlayGround>(); }
                
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
            //var toExecuteList = new List<PersistStrategyType>() {
            //    PersistStrategyType.File,
            //    PersistStrategyType.Db                
            //};                        
            //var executor = new CommandExecutor(toExecuteList, "Test message");

            //await executor.ExecuteCommandsAsync();
        }

    }
}
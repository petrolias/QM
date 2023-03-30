
using Microsoft.Extensions.DependencyInjection;
using QM.DAL.Abstractions;
using QM.DAL.Models;
using QM.Core.Abstractions.Enums;
using QM.Core.Extensions;
using Microsoft.Extensions.Logging;
using QM.Core;
using QM.Core.Abstractions;
using QM.Models.DomainModels;
using QM.Core.Helper;

namespace QM.TestProject
{

    public class PlayGround
    {          
        private IServiceProvider _serviceProvider { get; set; }
        private IRepository _repository { get => this._serviceProvider.GetRepository(); }
        private ILogger<PlayGround> _logger { get => this._serviceProvider.GetLogger<PlayGround>(); }
        private IConsumerPersistExecutor<PlayGround, RegistrationModel> _consumerPersistExecutor { 
            get => this._serviceProvider.GetConsumerPersistExecutor<PlayGround, RegistrationModel>(); }

        public PlayGround()
        {
            this._serviceProvider = new ServiceCollection()
                .AddDbRepository()
                .AddLoggerService()
                .AddConsumerPersistExecutor<PlayGround, RegistrationModel>()
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

        [Fact]
        public async void TestParameters()
        {
            var parameterHelper = new ParameterHelper();
            var paramA = parameterHelper.GetParameter(ParameterType.urlA);
            Assert.Equal("www.testA.com", paramA);           
        }

    }
}
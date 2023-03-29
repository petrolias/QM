using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QM.DAL;
using QM.DAL.Abstractions;
using QM.DAL.Models;
using QM.Core.Abstractions.Enums;
using QM.Core;
namespace QM.TestProject
{

    public class UnitTest1
    {
        private const string InMemoryDataBaseName = "InMemoryDatabase";
        private IServiceCollection _services { get; set; }
        private IRepository _repository { get => this._services.BuildServiceProvider().GetService<IRepository>() ?? throw new NullReferenceException(); }

        public UnitTest1()
        {                                    
            this._services = new ServiceCollection();
            this._services
                .AddDbContext<QMDBContext>(options => options.UseInMemoryDatabase(databaseName: InMemoryDataBaseName))
                .AddTransient<IRepository, Repository>();            
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
            var toExecuteList = new List<PersistSystemType>() {
                PersistSystemType.File,
                PersistSystemType.Db                
            };                        
            var executor = new CommandExecutor(toExecuteList, "Test message");

            await executor.ExecuteCommandsAsync();
        }

    }
}
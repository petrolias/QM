using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QM.DAL;
using QM.DAL.Abstractions;
using QM.DAL.Models;
using QM.Models.InputModels;

namespace QM.TestProject
{

    public class UnitTest1
    {
        private IServiceCollection _services { get; set; }
        private const string InMemoryDataBaseName = "InMemoryDatabase";
        public UnitTest1()
        {                                    
            this._services = new ServiceCollection();
            this._services
                .AddDbContext<QMDBContext>(options => options.UseInMemoryDatabase(databaseName: InMemoryDataBaseName))
                .AddTransient<IRepository, Repository>();            
        }
        [Fact]
        public void Test1()
        {
            var registrationModel = new RegistrationModelDB();
            var service = this._services.BuildServiceProvider().GetService<IRepository>()?? throw new NullReferenceException();
            service.SaveChangesAsync(registrationModel);
          

            // Assert that the entity was added successfully
            //Assert.NotEqual(0, myEntity.Id);

            //IFileStorage _fileStorage;

        }
    }
}
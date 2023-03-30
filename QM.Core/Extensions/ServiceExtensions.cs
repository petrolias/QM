using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QM.Core.Abstractions;
using QM.Mapper;
using QM.Mapper.Abstractions;
using QM.Models.Abstractions;
using QM.Models.DomainModels;
using Serilog;

namespace QM.Core.Extensions
{
    public static class ServiceExtensions
    {
        private const string InMemoryDataBaseName = "InMemoryDatabase";

        public static IServiceCollection AddDbRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection
               .AddDbContext<QMDBContext>(options => options.UseInMemoryDatabase(databaseName: InMemoryDataBaseName))
               .AddScoped<IRepository, Repository>();
            return serviceCollection;
        }

        public static IRepository GetRepository(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<IRepository>() ?? throw new NullReferenceException();
        }


        public static IServiceCollection AddConsumerPersistExecutor<TAppContext, TInputModel>(this IServiceCollection serviceCollection) where TInputModel : IRegistrationModel
        {
            serviceCollection
               .AddTransient<IConsumerPersistExecutor<TAppContext, TInputModel>, ConsumerPersistExecutor<TAppContext, TInputModel>>();
            return serviceCollection;
        }

        public static IConsumerPersistExecutor<TAppContext, TInputModel> GetConsumerPersistExecutor<TAppContext, TInputModel>(this IServiceProvider serviceProvider) where TInputModel : IRegistrationModel
        {
            return serviceProvider.GetService<IConsumerPersistExecutor<TAppContext, TInputModel>>() ?? throw new NullReferenceException();
        }

        public static IServiceCollection AddLoggerService(this IServiceCollection serviceCollection)
        {
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()               
               .WriteTo.Console()
               .CreateLogger();      

            serviceCollection
                .AddLogging(loggingBuilder =>
                {                    
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddSerilog(Log.Logger);
                });
            return serviceCollection;
        }

       
        public static ILogger<T> GetLogger<T>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<ILogger<T>>() ?? throw new NullReferenceException();

        }

    }
}

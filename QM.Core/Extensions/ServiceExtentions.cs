﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using QM.DAL;
using QM.DAL.Abstractions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Context;
using Serilog.Extensions.Logging;

namespace QM.Core.Extensions
{
    public static class ServiceExtentions
    {
        private const string InMemoryDataBaseName = "InMemoryDatabase";

        public static IServiceCollection AddDbRepository(this IServiceCollection serviceCollection)
        {
            serviceCollection
               .AddDbContext<QMDBContext>(options => options.UseInMemoryDatabase(databaseName: InMemoryDataBaseName))
               .AddScoped<IRepository, Repository>();
            return serviceCollection;
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

        public static IRepository GetRepository(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<IRepository>() ?? throw new NullReferenceException();
        }

        public static ILogger<T> GetLogger<T>(this IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService<ILogger<T>>() ?? throw new NullReferenceException();

        }

    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Gyldendal.Porter.Api.Configuration;
using Gyldendal.Porter.Application.Configuration;
using Gyldendal.Porter.Common.Configurations;
using Hangfire;
using Hangfire.Mongo;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Gyldendal.Porter.Application.CookingProcessor.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostContext, configurationBuilder) =>
                {
                    var environmentName = hostContext.HostingEnvironment.EnvironmentName;
                    configurationBuilder
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{environmentName}.json")
                        .AddEnvironmentVariables()
                        .Build();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    hostContext.Configuration.Bind(AppConfigurations.Configuration);
                    services.AddHostedService<CookingBackgroundServerWorker>();

                    var mongoUrlBuilder =
                        new MongoUrlBuilder(AppConfigurations.Configuration?.MongoDbConfig?.ConnectionString);
                    var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
                    GlobalConfiguration.Configuration.UseMongoStorage(mongoClient,
                        AppConfigurations.Configuration?.MongoDbConfig?.DbName, new MongoStorageOptions
                        {
                            Prefix = "porter.hangfire",
                            CheckConnection = true
                        });
                    MongoClassMapConfiguration.UsePorterMongoClassMaps();
                    services.UsePorterServiceComponents();
                    services.UsePorterMediatR();
                    services.UsePorterAutoMapping();

                    GlobalConfiguration.Configuration.UseActivator(
                        new HangfireActivator(services.BuildServiceProvider()));
                });
    }

    public class HangfireActivator : JobActivator
    {
        private readonly IServiceProvider _serviceProvider;

        public HangfireActivator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override object ActivateJob(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}
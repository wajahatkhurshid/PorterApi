using Gyldendal.Porter.Common.Configurations;
using Hangfire;
using Hangfire.Console;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Gyldendal.Porter.Api.HangFire
{
    /// <summary>
    /// Setting Up Hangfire
    /// </summary>
    public static class HangFireSetup
    {
        /// <summary>
        /// HangFire PipeLine Configuration
        /// </summary>
        public static void UsePorterHangfirePipeline(this IApplicationBuilder app)
        {
            var options = new BackgroundJobServerOptions { WorkerCount = 2, Queues = new[] { "default" } };
            app.UseHangfireServer(options);
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });
        }

        /// <summary>
        /// HangFire Services configurations
        /// </summary>
        public static void UsePorterHangfireServices(this IServiceCollection services)
        {
            var mongoUrlBuilder = new MongoUrlBuilder(AppConfigurations.Configuration.MongoDbConfig.ConnectionString);
            var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
            // Add Hangfire services. Hangfire.AspNetCore nuget required
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseConsole()
                //.UseFilter(new PreserveCookingQueueAttribute())
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, AppConfigurations.Configuration.MongoDbConfig.DbName,
                    new MongoStorageOptions
                    {
                        MigrationOptions = new MongoMigrationOptions
                        {
                            MigrationStrategy = new MigrateMongoMigrationStrategy(),
                            BackupStrategy = new CollectionMongoBackupStrategy()
                        },
                        Prefix = "porter.hangfire",
                        CheckConnection = true
                    })
            );
        }

        public static void UsePorterHangfireServer(this IServiceCollection services)
        {
            // Add the processing server as IHostedService
            services.AddHangfireServer(serverOptions => { serverOptions.ServerName = "Porter.Hangfire"; });
        }
    }
}
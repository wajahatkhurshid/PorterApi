using System;
using System.Reflection;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Application.Services;
using Gyldendal.Porter.Application.Services.Contributor;
using Gyldendal.Porter.Application.Services.EntityProcessing;
using Gyldendal.Porter.Application.Services.GpmMessageReceiver;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Application.Services.MerchandiseProduct;
using Gyldendal.Porter.Application.Services.Product;
using Gyldendal.Porter.Application.Services.SystemSeries;
using Gyldendal.Porter.Application.Services.WorkReview;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Configurations;
using Gyldendal.Porter.Domain.Contracts.Interfaces;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Infrastructure.ExternalClients.Datahub;
using Gyldendal.Porter.Infrastructure.ExternalClients.Gpm;
using Gyldendal.Porter.Infrastructure.ExternalClients.Interfaces;
using Gyldendal.Porter.Infrastructure.Repository;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Gyldendal.Porter.Infrastructure.Services;
using Hangfire;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using RapidCore.DependencyInjection;
using RapidCore.Locking;
using RapidCore.Migration;
using RapidCore.Mongo;
using RapidCore.Mongo.Migration;
using ILogger = Gyldendal.Porter.Common.ILogger;
using Logger = Gyldendal.Porter.Common.Logger;

namespace Gyldendal.Porter.Application.Configuration
{
    /// <summary>
    /// Initializes services
    /// </summary>
    public static class ServicesExtension
    {
        public static void UsePorterMediatR(this IServiceCollection services)
        {
            var assembly = AppDomain.CurrentDomain.Load("Gyldendal.Porter.Application.Services");
            //services.AddMediatR(cfg => cfg.Using<IMediator>().AsTransient(),assembly);
            services.AddMediatR(assembly);
            services.Replace(ServiceDescriptor.Transient<IMediator, Mediator>());
            services.Replace(ServiceDescriptor.Transient<INotificationHandler<EntityUpdateReceivedNotification>, EntityUpdateReceivedNotificationHandler>());
        }

        public static void UsePorterAutoMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(PorterMapper));
        }

        public static void UsePorterMongoMigrations(this IServiceCollection services, string environmentName)
        {
            services.AddSingleton<MigrationRunner>(c =>
            {
                var database = c.GetRequiredService<IMongoDatabase>();

                var connectionProvider = new ConnectionProvider();
                connectionProvider.Add("default", new MongoDbConnection(database), true);

                return new MigrationRunner(
                    c.GetService<ILogger<MigrationRunner>>(), //TODO : Need to find solution for this
                    new ServiceProviderRapidContainerAdapter(c),
                    new MigrationEnvironment(environmentName),
                    new NoopDistributedAppLockProvider(),
                    new MongoMigrationContextFactory(connectionProvider),
                    new ReflectionMigrationFinder(Assembly.GetExecutingAssembly()),
                    new MongoMigrationStorage()
                );
            });
        }

        public static void UsePorterServiceComponents(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddTransient<IContainerService, ContainerService>();
            services.AddTransient<IReplayService, ReplayService>();
            services.AddTransient<IEducationSubjectLevelsExtractorService, EducationSubjectLevelsExtractorService>();
            services.AddTransient<ISeriesAssemblerService, SeriesAssemblerService>();
            services.AddTransient<IInternetSubjectCookerService, InternetSubjectCookerService>();
            services.AddTransient<ISubscriptionService, SubscriptionService>(c => new SubscriptionService(c.GetService<ISubscriptionRepository>(), AppConfigurations.Configuration));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<ISeriesRepository, SeriesRepository>();
            services.AddTransient<IProductStockRepository, ProductStockRepository>();
            services.AddTransient<ITaxonomyResponseRepository, TaxonomyResponseRepository>();
            services.AddTransient<IMediaMaterialTypeRepository, MediaMaterialTypeRepository>();
            services.AddTransient<ISubjectCodeRepository, SubjectCodeRepository>();
            services.AddTransient<IImprintRepository, ImprintRepository>();
            services.AddTransient<ISupplyAvailabilityCodeRepository, SupplyAvailabilityCodeRepository>();
            services.AddTransient<IInternetCategoryRepository, InternetCategoryRepository>();
            services.AddTransient<IEducationSubjectLevelRepository, EducationSubjectLevelRepository>();
            services.AddTransient<IContributorRepository, ContributorRepository>();
            services.AddTransient<IWorkRepository, WorkRepository>();
            services.AddTransient<IWorkReviewRepository, WorkReviewRepository>();
            services.AddTransient<ICookedProductRepository, CookedProductRepository>();
            services.AddTransient<ICookedWorkReviewRepository, CookedWorkReviewRepository>();
            services.AddTransient<ICookedSeriesRepository, CookedSeriesRepository>();
            services.AddTransient<ICookedContributorRepository, CookedContributorRepository>();
            services.AddTransient<ISubscriptionRepository, SubscriptionRepository>();
            services.AddTransient<IMerchandiseProductRepository, MerchandiseProductRepository>();
            services.AddTransient<IProductCookingService, ProductCookingService>();
            services.AddTransient<ISeriesCookingService, SeriesCookingService>();
            services.AddTransient<IMerchandiseProductCookingService, MerchandiseProductCookingService>();
            services.AddTransient<IContributorCookingService, ContributorCookingService>();
            services.AddTransient<IWorkReviewCookingService, WorkReviewCookingService>();
            services.AddTransient<ICommonProductDataService, CommonProductDataService>();

            services.AddSingleton<LoggingManager.ILogger>(_ =>
                new LoggingManager.Logger(AppConfigurations.Configuration.LoggingManagerConfig));
            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IErrorResponseExtractor, ErrorResponseExtractor>();
            services.AddHttpClient<IProductStockClient, DatahubProductStockClient>(c =>
            {
                c.BaseAddress = new Uri(AppConfigurations.Configuration.DatahubApiConfig.ApiUrl);
            }).SetHandlerLifetime(TimeSpan.FromMinutes(5));
            services.AddHttpClient<IGpmApiClient, GpmApiClient>();
            services.AddTransient<GpmConfiguration>();
            services.AddTransient<ITaxonomyRepository, TaxonomyRepository>();

            services.AddSingleton<PorterContext>();
            // Singleton is recommended per the docs: http://mongodb.github.io/mongo-csharp-driver/2.0/reference/driver/connecting/#re-use
            // "It is recommended to store a MongoClient instance in a global place, either as a static variable or in an IoC container with a singleton lifetime."
            // The implementation of IMongoDatabase provided by a MongoClient is thread-safe and is safe to be stored globally or in an IoC container.
            services.AddSingleton(c =>
                {
                    var client = new MongoClient(AppConfigurations.Configuration.MongoDbConfig.ConnectionString);
                    return client.GetDatabase(AppConfigurations.Configuration.MongoDbConfig.DbName);
                }
            );
            services.AddTransient<IBackgroundJobClient>(c => new BackgroundJobClient(JobStorage.Current));
        }

        public static void UsePorterAzureServiceComponents(this IServiceCollection services)
        {
            services.AddTransient<IReceiveGpmMessageService, ReceiveGpmMessageService>();
        }
    }
}
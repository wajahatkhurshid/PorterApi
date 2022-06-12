using System;
using System.IO;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Configuration;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Common.Configurations;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gyldendal.Porter.Tests.IntegrationTests.PorterApi
{
    public class PorterApiTestSetup
    {
        private static IConfiguration _configuration;
        private static IServiceScopeFactory _scopeFactory;
        
        public void RunBeforeQueryHandlerTests()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            var services = new ServiceCollection();
            
            _configuration.Bind(AppConfigurations.Configuration);

            /*services.AddTransient<PorterContext>();
            services.AddTransient<IMediaMaterialTypeRepository, MediaMaterialTypeRepository>();
            services.AddTransient<IInternetCategoryRepository, InternetCategoryRepository>();
            services.AddTransient<IEducationSubjectLevelRepository, EducationSubjectLevelRepository>();*/
            services.UsePorterServiceComponents();
            services.AddMediatR( AppDomain.CurrentDomain.Load("Gyldendal.Porter.Application.Services"));
            services.AddAutoMapper(typeof(PorterMapper));

            _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>();
        }

        public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            return await mediator.Send(request);
        }
    }
}

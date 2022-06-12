using Gyldendal.Porter.Api.Extensions;
using Gyldendal.Porter.Common.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text.Json.Serialization;
using Gyldendal.Porter.Api.Configuration;
using Gyldendal.Porter.Api.HangFire;
using Gyldendal.Porter.Api.Middleware;
using Gyldendal.Porter.Application.Configuration;
using Microsoft.Extensions.Azure;

namespace Gyldendal.Porter.Api
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        private IWebHostEnvironment _environment;
        /// <summary>
        /// Startup 
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="environment"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            configuration.Bind(AppConfigurations.Configuration);
            Configuration = configuration;
            _environment = environment;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        ///  This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddControllers().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddServerSideBlazor();
            MongoClassMapConfiguration.UsePorterMongoClassMaps();
            services.UsePorterServiceComponents();
            services.UsePorterMediatR();
            services.UsePorterHangfireServices();
            services.UsePorterHangfireServer();
            services.UsePorterAutoMapping();
            services.UsePorterAzureServiceComponents();
            services.UsePorterMongoMigrations(_environment.EnvironmentName);

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.UseAllOfToExtendReferenceSchemas();
            });

            if (!_environment.IsDevelopment())
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
                    options.HttpsPort = 443;
                });
            }

            services.AddAzureClients(clientsBuilder =>
            {
                clientsBuilder
                    .AddServiceBusClient(AppConfigurations.Configuration.GpmConfig.ServiceBusConnectionString)
                    .WithName(AppConfigurations.Configuration.GpmConfig.ServiceBusClient)
                    .ConfigureOptions(options =>
                    {
                        options.RetryOptions.Delay = TimeSpan.FromMilliseconds(50);
                        options.RetryOptions.MaxDelay = TimeSpan.FromSeconds(5);
                        options.RetryOptions.MaxRetries = 3;
                    });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="appLifetime"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime appLifetime)
        {
            app.UseMiddleware<HttpLoggingMiddleware>();
            app.UseHttpsRedirection();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Porter API v1");
            });
            app.UsePorterHangfirePipeline();
            app.ConfigureRequestPipeline(appLifetime);
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages(); // existing endpoints
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

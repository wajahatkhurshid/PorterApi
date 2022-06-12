using Gyldendal.Porter.Api.HangFire;
using Gyldendal.Porter.Api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Gyldendal.Porter.Api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MiddlewareExtension
    {
        /// <summary>
        /// Request Pipeline Configuration
        /// </summary>
        /// <param name="app"></param>
        /// <param name="appLifetime"></param>
        /// <returns></returns>
        public static IApplicationBuilder ConfigureRequestPipeline(this IApplicationBuilder app,
            IHostApplicationLifetime appLifetime)
        {
            app.UseExceptionHandling();
            app.UseHttpsRedirection();
            app.UseRouting();

            HangfireJobs.ConfigureHangfireJobs();
            MongoMigrationRunner.RunMigration(
                app.ApplicationServices, appLifetime
            );

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }

        /// <summary>
        /// Adds middleware to handle exceptions and log exception details.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
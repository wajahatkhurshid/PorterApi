using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RapidCore.Migration;
using RapidCore.Threading;
using System;

namespace Gyldendal.Porter.Api.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class MongoMigrationRunner
    {
        /// <summary>
        /// Run all the Migrations in the start of application
        /// </summary>
        /// <param name="container"></param>
        /// <param name="appLife"></param>
        public static void RunMigration(IServiceProvider container, IHostApplicationLifetime appLife)
        {
            try
            {
                var migrationRunner = container.GetRequiredService<MigrationRunner>();
                migrationRunner.UpgradeAsync().AwaitSync();
            }
            catch (Exception ex)
            {
                appLife.StopApplication();
            }
        }
    }
}

using Gyldendal.Porter.Common.Configurations;
using Hangfire;
using System;
using Gyldendal.Porter.Application.HangfireJobs;

namespace Gyldendal.Porter.Api.HangFire
{
    /// <summary>
    /// Configuration of HangFire Jobs
    /// </summary>
    public class HangfireJobs
    {
        /// <summary>
        /// Triggering Hangfire Jobs startup method on specified CRON expression 
        /// </summary>
        public static void ConfigureHangfireJobs()
        {
            RecurringJob.AddOrUpdate<ProductStockUpdateJob>(AppConfigurations.ProductStockUpdateJob,
                p => p.Execute(null), AppConfigurations.Configuration.HangFireConfig.HangfireJobCron,
                TimeZoneInfo.FindSystemTimeZoneById(AppConfigurations.DanishTimeZoneId));

            RecurringJob.AddOrUpdate<MediaMaterialTypeTaxonomyUpdateJob>(AppConfigurations.MediaMaterialTypeTaxonomyUpdateJob,
                p => p.Execute(null), AppConfigurations.Configuration.HangFireConfig.HangfireJobCron,
                TimeZoneInfo.FindSystemTimeZoneById(AppConfigurations.DanishTimeZoneId));

            RecurringJob.AddOrUpdate<SubjectCodeUpdateJob>(AppConfigurations.SubjectCodeTaxonomyUpdateJob,
                p => p.Execute(null), AppConfigurations.Configuration.HangFireConfig.HangfireJobCron,
                TimeZoneInfo.FindSystemTimeZoneById(AppConfigurations.DanishTimeZoneId));

            RecurringJob.AddOrUpdate<SupplyAvailabilityCodeUpdateJob>(AppConfigurations.SupplyAvailabilityCodeTaxonomyUpdateJob,
                p => p.Execute(null), AppConfigurations.Configuration.HangFireConfig.HangfireJobCron,
                TimeZoneInfo.FindSystemTimeZoneById(AppConfigurations.DanishTimeZoneId));

            RecurringJob.AddOrUpdate<InternetCategoryTaxonomyUpdateJob>(AppConfigurations.InternetCategoryTaxonomyUpdateJob,
                p => p.Execute(null), AppConfigurations.Configuration.HangFireConfig.HangfireJobCron,
                TimeZoneInfo.FindSystemTimeZoneById(AppConfigurations.DanishTimeZoneId));

            RecurringJob.AddOrUpdate<ImprintJob>(AppConfigurations.ImprintTaxonomyUpdateJob,
                p => p.Execute(null), AppConfigurations.Configuration.HangFireConfig.HangfireJobCron,
                TimeZoneInfo.FindSystemTimeZoneById(AppConfigurations.DanishTimeZoneId));

            RecurringJob.AddOrUpdate<EducationSubjectLevelTaxonomyUpdateJob>(AppConfigurations.EducationSubjectLevelTaxonomyUpdateJob,
                p => p.Execute(null), AppConfigurations.Configuration.HangFireConfig.HangfireJobCron,
                TimeZoneInfo.FindSystemTimeZoneById(AppConfigurations.DanishTimeZoneId));

            RecurringJob.AddOrUpdate<ServicebusListenerJob>("ServiceBusListenerJob",
                p => p.Execute(null), AppConfigurations.Configuration.HangFireConfig.HangfireJobCron,
                TimeZoneInfo.FindSystemTimeZoneById(AppConfigurations.DanishTimeZoneId));
        }
    }
}

using Hangfire.Dashboard;

namespace Gyldendal.Porter.Api.HangFire
{
    /// <summary>
    /// Hangfire Authorization Filter
    /// </summary>
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <summary>
        /// Hangfire Dashboard Context for Authorization
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}

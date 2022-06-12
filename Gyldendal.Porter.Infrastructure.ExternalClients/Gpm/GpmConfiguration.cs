using System;
using Gyldendal.Porter.Common.Configurations;

namespace Gyldendal.Porter.Infrastructure.ExternalClients.Gpm
{
    public class GpmConfiguration
    {
        public Uri BaseUri => new Uri(AppConfigurations.Configuration.GpmConfig.GpmUrl);
        public string Username => "swagger";
        public string Role => "SuperAdmin";
    }
}

using System.Collections.Generic;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class Subscription
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string PorterEndpoint { get; set; }

        public string GpmUrl { get; set; }

        public List<Scope> Scopes { get; set; }
    }
}

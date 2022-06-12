using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.Entities.Subscription
{
    public class Subscription : DomainEntityBase
    {
        public string Name { get; set; }
        public string GpmUrl { get; set; }
        public List<Scope> Scopes { get; set; }
    }
}

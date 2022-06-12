using System;

namespace Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy
{
    public class TaxonomyResponse : DomainEntityBase
    {
        public int TaxonomyId { get; set; }
        public string ResponsePayload { get; set; }
        public DateTime Created { get; set; }
    }
}

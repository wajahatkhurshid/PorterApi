using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy
{
    public class Taxonomy
    {
        public int Id { get; set; }
        public List<int> RootNodeIds { get; set; }
        public List<TaxonomyNode> TaxonomyNodes { get; set; }
    }
}

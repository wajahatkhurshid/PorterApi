using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy
{
    public class TaxonomyNode
    {
        public int NodeId { get; set; }
        public string Name { get; set; }
        public List<int> ChildNodeIds { get; set; }
        public int? ParentNodeId { get; set; }
        public int Level { get; set; }
    }
}

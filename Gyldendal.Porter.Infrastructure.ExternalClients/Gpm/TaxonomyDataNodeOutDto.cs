namespace Gyldendal.Porter.Infrastructure.ExternalClients.Gpm
{
    public partial class TaxonomyDataNodeOutDto
    {
        public int NodeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public bool IsSelectable { get; set; }
        public System.Collections.Generic.ICollection<int> ChildrenIds { get; set; }
        public int? ParentNodeId { get; set; }
        public int Level { get; set; }
    }
}

namespace Gyldendal.Porter.Domain.Contracts.Entities.MasterData
{
    public class MediaMaterialType : DomainEntityBase
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public MediaMaterialType Parent { get; set; }
    }
}
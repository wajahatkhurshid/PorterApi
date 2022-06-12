
namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class MediaMaterialType
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public MediaMaterialType Parent { get; set; }
    }
}

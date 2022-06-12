using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class EducationLevel
    {
        public int Id { get; set; }

        public int? LevelNumber { get; set; }

        public WebShop WebShop { get; set; }

        public int? AreaId { get; set; }

        public string Name { get; set; }
    }
}

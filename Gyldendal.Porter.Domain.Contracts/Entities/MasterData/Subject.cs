using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Domain.Contracts.Entities.MasterData
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public WebShop WebShop { get; set; }

        public int? AreaId { get; set; }
    }
}

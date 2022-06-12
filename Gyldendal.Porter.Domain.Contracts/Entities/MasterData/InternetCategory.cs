using Gyldendal.Porter.Domain.Contracts.Enums;

namespace Gyldendal.Porter.Domain.Contracts.Entities.MasterData
{
    public class InternetCategory : DomainEntityBase
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public InternetCategoryTypes Type { get; set; }

        public InternetCategory Parent { get; set; }
    }
}
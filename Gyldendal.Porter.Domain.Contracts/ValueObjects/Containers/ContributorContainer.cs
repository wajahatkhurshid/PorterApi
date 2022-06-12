using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class ContributorContainer : ContainerBase
    {
        public string ContributorId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string BiographyText { get; set; }

        public string PhotoUrl { get; set; }

        public int ContributorTypeId { get; set; }

        public List<int> WorkIds { get; set; }

        public List<int> ProductIds { get; set; }

        public bool IsDeleted { get; set; }
    }
}

using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class ProfileContributorAuthorContainer : ContainerInstanceBase
    {
        public string Phase { get; set; }

        public string PhaseState { get; set; }

        public object ProfileID { get; set; }

        public string ProfileName { get; set; }

        public string ProfileImageUrl { get; set; }

        public string ProfileText { get; set; }

        public List<ProfileContributorMember> ProfileContributorMembers { get; set; }
    }
}

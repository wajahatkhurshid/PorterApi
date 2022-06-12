namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class ProfileContributorMember : ContainerInstanceBase
    {
        public string Phase { get; set; }
        
        public string PhaseState { get; set; }
        
        public object ProfileMemberId { get; set; }
        
        public string ProfileMemberDisplayName { get; set; }
        
        public string ProfileMemberFirstName { get; set; }
        
        public string ProfileMemberSecondName { get; set; }
    }
}

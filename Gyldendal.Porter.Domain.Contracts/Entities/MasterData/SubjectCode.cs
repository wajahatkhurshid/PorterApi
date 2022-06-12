namespace Gyldendal.Porter.Domain.Contracts.Entities.MasterData
{
    public class SubjectCode : DomainEntityBase
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public SubjectCode Parent { get; set; }
    }
}
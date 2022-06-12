namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class SubjectCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public SubjectCode Parent { get; set; }
    }
}
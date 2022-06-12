namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class Attachment
    {
        public string SampleUrl { get; set; }
        public string Description { get; set; }
        public string PublizonIdentifier { get; set; }
        public bool IsSecured { get; set; }
        public bool IsDeleted { get; set; }
    }
}

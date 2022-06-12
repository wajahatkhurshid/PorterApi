namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class MarketingAttachment : ContainerInstanceBase
    {
        public string AttachmentTitle { get; set; }

        public string AttachmentFileType { get; set; }

        public string AttachmentDescription { get; set; }
    }
}

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class WorkReviewContainer : ContainerBase
    {
        public string ReviewMedia { get; set; }

        public int ReviewRating { get; set; }

        public string ReviewWrittenBy { get; set; }

        public string ReviewContent { get; set; }
    }
}

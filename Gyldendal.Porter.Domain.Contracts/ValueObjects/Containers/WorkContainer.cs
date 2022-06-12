using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class WorkContainer : ContainerBase
    {
        public string WorkIdentificationNumber { get; set; }

        public string WorkTitle { get; set; }

        public string WorkSubtitle { get; set; }

        public string WorkWebText { get; set; }

        public string WorkGyldendalShopText { get; set; }

        public string WorkInternalText { get; set; }

        public List<int> ProductIds { get; set; }

        public List<int> MerchandiseIds { get; set; }

        public List<int> WorkCollectionTitleIds { get; set; }

        public List<int> WorkContributorAuthorIds { get; set; }

        public List<List<GpmNode>> WorkSubjectCodeMain { get; set; }

        public List<List<GpmNode>> WorkSubjectCode { get; set; }

        public List<List<GpmNode>> WorkGUInternetSubject { get; set; }

        public List<List<GpmNode>> WorkEducationSubjectLevel { get; set; }

        public List<WorkReviewContainer> WorkReviews { get; set; }
    }
}

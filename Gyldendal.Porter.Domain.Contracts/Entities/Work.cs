using System;
using System.Collections.Generic;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Domain.Contracts.Entities
{
    public class Work : DomainEntityBase
    {
        public int ContainerInstanceId { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string WorkWebText { get; set; }

        public string WorkGyldendalShopText { get; set; }

        public string WorkInternalText { get; set; }

        public List<int> ProductIds { get; set; }

        public List<List<GpmNode>> WorkSubjectCodeMain { get; set; }

        public List<List<GpmNode>> WorkSubjectCode { get; set; }

        public List<List<GpmNode>> WorkGUInternetSubject { get; set; }

        public List<List<GpmNode>> WorkEducationSubjectLevel { get; set; }

        public List<WorkReviewContainer> WorkReviews { get; set; }

        public DateTime UpdatedTimestamp { get; set; }
    }
}
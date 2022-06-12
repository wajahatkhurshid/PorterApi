using System;

namespace Gyldendal.Porter.SolrMonitoring.WorkReviews.Models
{
    public class WorkReview
    {
        public string id { get; set; }
        public int workReviewId { get; set; }
        public int workId { get; set; }
        public int websiteId { get; set; }
        public int reviewAttributeId { get; set; }
        public bool draft { get; set; }
        public DateTime updatedTimestamp { get; set; }
        public int Rating { get; set; }
        public string review { get; set; }
        public string shortDescription { get; set; }
        public string titleda { get; set; }
        public string aboutAuthor { get; set; }
        public string textType { get; set; }
        public int priority { get; set; }
	}
}

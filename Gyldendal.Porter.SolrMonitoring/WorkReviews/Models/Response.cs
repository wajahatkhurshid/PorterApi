
using System.Collections.Generic;


namespace Gyldendal.Porter.SolrMonitoring.WorkReviews.Models
{
    public class WorkReviewResponse
    {
        public int numFound { get; set; }
        public List<WorkReview> docs { get; set; }
    }

    public class SolrWorkReviewQueryResponseBody
    {
        public WorkReviewResponse response { get; set; }
    }
}

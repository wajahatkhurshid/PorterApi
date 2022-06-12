using System.Collections.Generic;

namespace Gyldendal.Porter.SolrMonitoring.Contributor.Models
{
    public class ContributorResponse
    {
        public int numFound { get; set; }
        public List<Contributor> docs { get; set; }
    }

    public class SolrContributorQueryResponseBody
    {
        public ContributorResponse response { get; set; }
    }
}

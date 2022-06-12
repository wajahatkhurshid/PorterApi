using System;

namespace Gyldendal.Porter.SolrMonitoring.Contributor.Models
{
    public class Contributor
    {
        public string id { get; set; }
        public string contributorid { get; set; }
        public string description { get; set; }
        public string contributorName { get; set; }
        public string imagesJson { get; set; }
        public int websiteId { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string searchname { get; set; }
        public DateTime updatedTimestamp { get; set; }
        public string[] exactmatchfield { get; set; }
        public string[] substringfield { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.Entities
{
    public class WorkReview : DomainEntityBase
    {
        public string WorkReviewId { get; set; }
        
        public string WorkId { get; set; }
        
        public DateTime UpdatedTimestamp { get; set; }
        
        public int? Rating { get; set; }
        
        public string Review { get; set; }
        
        public string Title { get; set; }
        
        public string AuthorInfo { get; set; }
        
        public string Version { get; set; }
        
        public List<string> WebShopIds { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}

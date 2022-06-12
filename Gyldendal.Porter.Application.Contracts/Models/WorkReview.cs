using System;
using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class WorkReview
    {
        public string Id { get; set; }
        
        public string WorkId { get; set; }

        public string ReviewMedia { get; set; }

        public DateTime UpdatedTimestamp { get; set; }
        
        public int? Rating { get; set; }
        
        public string Review { get; set; }
        
        public string AuthorInfo { get; set; }

        public List<WebShop> WebShops { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}

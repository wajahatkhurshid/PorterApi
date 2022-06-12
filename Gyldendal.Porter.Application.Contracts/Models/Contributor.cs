using System;
using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class Contributor
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BiographyText { get; set; }
        public string Bibliography { get; set; }
        public string PhotoUrl { get; set; }
        public int ContributorTypeId { get; set; }
        public DateTime UpdatedTimestamp { get; set; }
        public List<WebShop> WebShops { get; set; }
    }
}

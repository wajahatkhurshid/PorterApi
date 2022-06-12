using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Common.Enums;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Common.Request
{
    public class SearchProductRequest
    {
        /// <summary>
        /// Isbn
        /// </summary>
        [Required]
        [JsonProperty(Required = Required.DisallowNull)]
        public string Isbn { get; set; }

        public List<string> Isbns { get; set; }

        /// <summary>
        /// Include comma separated Property names from response model like "Isbn,Title"
        /// </summary>
        public string PropertiesToInclude { get; set; }

        /// <summary>
        /// Group Type for the data filtration
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// GradeLevel for the data filtration
        /// </summary>
        public string AuthorName { get; set; }

        /// <summary>
        /// ClassName for the data filtration
        /// </summary>
        public string MediaType { get; set; }

        public WebShop WebShop { get; set; }

        public string SubTitle { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 100;

        public ProductSortByOptions ProductSortByOptions { get; set; }

        public SortBy SortBy { get; set; }

        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}

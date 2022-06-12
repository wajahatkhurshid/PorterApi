using System;
using System.Collections.Generic;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Domain.Contracts.Entities
{
    public class Product : DomainEntityBase
    {
        public int ContainerInstanceId { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public string WebText { get; set; }

        public List<List<GpmNode>> MediaMaterialType { get; set; }

        public int? WorkId { get; set; }

        public string FirstPrintPublishDate { get; set; }

        public DateTime? CurrentPrintRunPublishDate { get; set; }

        /// <summary>
        /// Sample Url
        /// </summary>
        public string ReadingSamples { get; set; }

        public int? Edition { get; set; }

        public int? NoOfPages { get; set; }

        /// <summary>
        /// Used in Trade specifically for GPlus WebShops
        /// </summary>
        public List<List<GpmNode>> ExcuseCode { get; set; }

        public List<List<GpmNode>> ProductEducationSubjectLevels { get; set; }

        public List<List<GpmNode>> ProductGUInternetSubjects { get; set; }

        public List<List<GpmNode>> ProductSubjectCodesMain { get; set; }

        public List<List<GpmNode>> ProductSubjectCodes { get; set; }

        /// <summary>
        /// Redaktion
        /// </summary>
        public List<List<GpmNode>> EditorialStaff { get; set; }

        /// <summary>
        /// Forlag - Used in Trade specifically for GPlus WebShops
        /// </summary>
        public string Publisher { get; set; }

        public string DurationInMinutes { get; set; }

        public int? Stock { get; set; }

        public List<ProductCollectionTitleContainer> Series { get; set; }

        public List<int> SeriesIds { get; set; }

        public List<MarketingAttachment> Attachments { get; set; }

        public List<ProfileContributorAuthorContainer> ContributorAuthors { get; set; }

        public List<int> ContributorIds { get; set; }

        public bool IsNextPrintRunPlanned { get; set; }

        public string Url { get; set; }

        public int? MaterialTypeRank { get; set; }

        public int? MediaTypeRank { get; set; }

        public string Height { get; set; }

        public string Width { get; set; }

        public string ThicknessDepth { get; set; }

        /// <summary>
        /// gennemsynseksemplar
        /// </summary>
        public bool ReviewCopy { get; set; }

        public List<List<GpmNode>> Websites { get; set; }

        public List<List<GpmNode>> Imprint { get; set; }

        /// <summary>
        /// VejledendePris - Used as Price without VAT
        /// </summary>
        public decimal? IndicativePrice { get; set; }

        /// <summary>
        /// PrisUdenMoms - Price without VAT used in GU
        /// </summary>
        public decimal? PriceWithoutVat { get; set; }

        public decimal? PriceWithVat { get; set; }

        /// <summary>
        /// When an update was last received on the contributor
        /// </summary>
        public DateTime UpdatedTimestamp { get; set; }

        public string WebsiteAddress { get; set; }

        public string ProductType { get; set; }

        /// <summary>
        /// Marks if the contributor's data should be deleted
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}

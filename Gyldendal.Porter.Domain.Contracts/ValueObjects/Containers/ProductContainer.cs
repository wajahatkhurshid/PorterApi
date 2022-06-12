using System;
using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class ProductContainer : ContainerBase
    {
        public string ProductISBN13 { get; set; }

        public string ProductTitle { get; set; }

        public string ProductSubtitle { get; set; }

        public string ProductGyldendalShopText { get; set; }

        public string ProductWebText { get; set; }

        public List<List<GpmNode>> ProductMediaMaterialeType { get; set; }

        public int? WorkId { get; set; }

        public string RelatedProductFirstEditionProductPublishedDate { get; set; }

        public DateTime? ProductPublishedDate { get; set; }

        /// <summary>
        /// Sample Url
        /// </summary>
        public string ProductGUSampleURL { get; set; }

        public int? ProductEdition { get; set; }

        public int? ProductExtentTotalPageCount { get; set; }

        /// <summary>
        /// Used in Trade specifically for GPlus WebShops
        /// </summary>
        public List<List<GpmNode>> ProductSupplyAvailabilityCode { get; set; }

        public List<List<GpmNode>> ProductEducationSubjectLevel { get; set; }

        public List<List<GpmNode>> ProductGUInternetSubject { get; set; }

        public List<List<GpmNode>> ProductSubjectCodeMain { get; set; }

        public List<List<GpmNode>> ProductSubjectCode { get; set; }

        /// <summary>
        /// Redaktion
        /// </summary>
        public List<List<GpmNode>> ProductEditorialDivision { get; set; }

        /// <summary>
        /// Forlag - Used in Trade specifically for GPlus WebShops
        /// </summary>
        public string RelatedProductOriginalProductPublisher { get; set; }

        public string ProductExtentDuration { get; set; }

        public int? Stock { get; set; }

        public List<int> ProductCollectionTitleIds { get; set; }

        public List<MarketingAttachment> MarketingAttachment { get; set; }

        public List<int> ProductContributorAuthorIds { get; set; }

        public bool IsNextPrintRunPlanned { get; set; }

        public string ProductGUProductURL { get; set; }

        public int? MaterialTypeRank { get; set; }

        public int? MediaTypeRank { get; set; }

        public string ProductMeasurementHeight { get; set; }

        public string ProductMeasurementWidth { get; set; }

        public string ProductMeasurementThickness { get; set; }

        /// <summary>
        /// gennemsynseksemplar
        /// </summary>
        public bool ProductGUEnableInspectionCopy { get; set; }

        public List<List<GpmNode>> ProductDisplayOnShops { get; set; }

        public List<List<GpmNode>> ProductImprint { get; set; }

        /// <summary>
        /// VejledendePris - Used as Price without VAT
        /// </summary>
        public decimal? ProductSupplyPriceWithoutVAT { get; set; }

        /// <summary>
        /// PrisUdenMoms - Price without VAT used in GU
        /// </summary>
        public decimal? PriceWithoutVat { get; set; }

        public decimal? ProductSupplyPriceWithVAT { get; set; }

        public string ProductType { get; set; }

        /// <summary>
        /// Marks if the contributor's data should be deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public string FaultReason { get; set; }
    }
}

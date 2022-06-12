using System;
using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string Isbn { get; set; }

        public string Title { get; set; }

        public string Subtitle { get; set; }

        public string Description { get; set; }

        public string WebText { get; set; }

        public string MaterialType { get; set; }

        public string MediaType { get; set; }

        public int? WorkId { get; set; }

        public string WorkTitle { get; set; }

        public string WorkDescription { get; set; }

        public List<EducationLevel> ProductEducationSubjectLevels { get; set; }

        public DateTime? FirstPrintPublishDate { get; set; }

        public DateTime? CurrentPrintRunPublishDate { get; set; }

        /// <summary>
        /// Sample Url
        /// </summary>
        public string ReadingSamples { get; set; }

        public string SeoText { get; set; }

        public int? Edition { get; set; }

        public int? NoOfPages { get; set; }

        public string ExcuseCode { get; set; }

        /// <summary>
        /// Redaktion
        /// </summary>
        public string EditorialStaff { get; set; }

        /// <summary>
        /// Forlag - Used in Trade specifically for GPlus WebShops
        /// </summary>
        public string Publisher { get; set; }

        public string DurationInMinutes { get; set; }

        public int? Stock { get; set; }

        public List<Series> Series { get; set; }

        public List<Contributor> Contributors { get; set; }

        public bool IsNextPrintRunPlanned { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// CDS set this by checking  is_secured & kd_slettet in DEA_KDWS_GDKattachments table from KD
        /// TODO: Should we maintain attachment collection as well?
        /// </summary>
        public List<Attachment> Attachments { get; set; }

        public int? MaterialTypeRank { get; set; }

        public int? MediaTypeRank { get; set; }

        public double? GrossWeight { get; set; }

        public double? NetWeight { get; set; }

        public int? Height { get; set; }

        public int? Width { get; set; }

        public int? ThicknessDepth { get; set; }

        /// <summary>
        /// gennemsynseksemplar
        /// </summary>
        public decimal? ReviewCopy { get; set; }

        public List<WebShop> WebShops { get; set; }

        public string Imprint { get; set; }

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

        public bool InspectionCopyAllowed { get; set; }

        public string AccessControl { get; set; }

        public string WebsiteAddress { get; set; }

        public string ProductType { get; set; }

        /// <summary>
        /// Marks if the contributor's data should be deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public List<Level> Levels { get; set; }

        public List<Area> Areas { get; set; }

        public List<SubArea> SubAreas { get; set; }
        
        public List<Subject> Subjects { get; set; }

        public List<SubjectCode> SubjectCodes { get; set; }
    }
}

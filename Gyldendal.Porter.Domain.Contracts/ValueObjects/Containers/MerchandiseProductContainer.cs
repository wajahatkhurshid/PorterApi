using System;
using System.Collections.Generic;

namespace Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers
{
    public class MerchandiseProductContainer : ContainerBase
    {
        public string MerchandiseEan { get; set; }
        public string MerchandiseTitle { get; set; }
        public string MerchandiseSubtitle { get; set; }
        public List<List<GpmNode>> Imprint { get; set; }

        /// <summary>
        /// Redaktion
        /// </summary>
        public List<List<GpmNode>> MerchandiseEditorialDivision { get; set; }
        public List<List<GpmNode>> MerchandiseManagingEditor { get; set; }
        public List<List<GpmNode>> MerchandiseEditorialResponsible { get; set; }
        public List<List<GpmNode>> MerchandiseProductionResponsible { get; set; }
        public List<List<GpmNode>> MerchandiseMarketingResponsible { get; set; }
        public List<List<GpmNode>> MerchandiseContactResponsible { get; set; }
        public List<List<GpmNode>> MerchandiseMaterialeType { get; set; }
        public string MerchandiseMeasurementHeight { get; set; }
        public string MerchandiseMeasurementWidth { get; set; }
        public bool MerchandisePrintOnDemand { get; set; }
        public List<List<GpmNode>> MerchandiseSubjectCodeMain { get; set; }
        public List<List<GpmNode>> MerchandiseSubjectCode { get; set; }
        public List<List<GpmNode>> MerchandiseGUInternetSubject { get; set; }
        public List<List<GpmNode>> MerchandiseAudienceRange { get; set; }
        public List<List<GpmNode>> MerchandiseEducationSubjectLevel { get; set; }
        public string MerchandiseWebText { get; set; }
        public string MerchandiseGyldendalShopText { get; set; }
        public string MerchandiseInternalText { get; set; }
        public float MerchandiseSupplyPriceWithoutVAT { get; set; }
        public float MerchandiseSupplyPriceWithVAT { get; set; }
        public List<List<GpmNode>> MerchandiseSupplyDiscountCode { get; set; }
        public string MerchandisePublishedQuarter { get; set; }
        public DateTime MerchandisePublishedDate { get; set; }
        public int MerchandiseCopies { get; set; }
        public int MerchandiseFreeSampleCopies { get; set; }
        public bool MerchandiseAtypicalProduction { get; set; }
        public string MerchandiseRightsTransferredTo { get; set; }
        public DateTime MerchandiseDecommissionDate { get; set; }
        public string MerchandiseFunding { get; set; }
        public List<List<GpmNode>> MerchandiseBusinessIntelligenceTrackingTaxonomies { get; set; }
        public string MerchandiseBusinessIntelligenceTags { get; set; }
        public int MerchandiseBatchNr { get; set; }
        public DateTime MerchandiseBatchPublishingDate { get; set; }
        public string MerchandiseBatchCalculationNumber { get; set; }
        public float MerchandiseBatchSupplyPriceWithoutVAT { get; set; }
        public int MerchandiseBatchCopies { get; set; }
        public string MerchandiseBatchResponsible { get; set; }
        public string MerchandiseCoverImageView { get; set; } // TODO: Really not sure about this
        public List<ProfileContributorAuthorContainer> MerchandiseContributorAuthor { get; set; }
        public List<ProductCollectionTitleContainer> MerchandiseCollectionTitle { get; set; }
        public List<List<GpmNode>> MerchandiseDisplayOnShops { get; set; }
        public bool MerchandiseTermination { get; set; }
        public List<List<GpmNode>> MerchandiseImprint { get; set; }

    }
}

using System;

namespace Gyldendal.Porter.SolrMonitoring.Product.Models
{
	public class Product
	{
		public string id { get; set; }

		public string description { get; set; }

		public int duration { get; set; }

		public int edition { get; set; }

		public string excuseCode { get; set; }


		public bool inStock { get; set; }


		public bool isNextPrintRunPlanned { get; set; }


		public bool isSaleConfigAvailable { get; set; }


		public bool inspectionCopyAllowed { get; set; }


		public int pages { get; set; }


		public string productId { get; set; }


		public float discountPercentage { get; set; }


		public bool hasOtherDiscount { get; set; }


		public string sampleUrl { get; set; }


		public string seoText { get; set; }

		/// <summary>
		/// Index only field, not for retrieving purposes.
		/// </summary> 

		public int[] seriesIds { get; set; }


		public int sroductSource { get; set; }


		public string[] seriesNames { get; set; }


		public string subtitle { get; set; }


		public string[] systemNames { get; set; }


		public string sitle { get; set; }


		public int workId { get; set; }


		public string workText { get; set; }


		public string workTitle { get; set; }


		public bool hasVideos { get; set; }


		public bool hasImages { get; set; }


		public string originalCoverImageUrl { get; set; }


		public bool isPhysical { get; set; }


		public string[] contributorIds { get; set; }


		public string[] membershipPaths { get; set; }


		public DateTime updatedTimestamp { get; set; }


		public string[] bundleProductTitles { get; set; }


		public string[] bundleProductIsbns { get; set; }


		public int productType { get; set; }


		public string reviewText { get; set; }


		public string[] areas { get; set; }


		public string[] authorNames { get; set; }


		public string isbn13 { get; set; }


		public string[] levels { get; set; }


		public string materialTypeName { get; set; }


		public string mediaTypeName { get; set; }


		public DateTime publishDate { get; set; }


		public DateTime currentPrintRunPublishDate { get; set; }


		public string physicalIsbn { get; set; }


		public int publisherId { get; set; }


		public string publisher { get; set; }


		public string[] subAreas { get; set; }

		/// <summary>
		/// Index only field, not for retrieving purposes.
		/// </summary> 

		public string[] subjects { get; set; }


		public string[] subjectWithAreaAndSubarea { get; set; }

		/// <summary>
		/// Index only field, not for retrieving purposes.
		/// </summary> 

		public string[] themaCodes { get; set; }


		public int websiteId { get; set; }


		public int mediaTypeRank { get; set; }


		public int materialTypeRank { get; set; }


		public double grossWeight { get; set; }


		public double netWeight { get; set; }


		public int height { get; set; }


		public int width { get; set; }


		public int thicknessDepth { get; set; }


		public bool isSupplementaryMaterial { get; set; }


		public string serializedAreasInfo { get; set; }


		public string serializedContributorsInfo { get; set; }


		public string serializedCoverImagesInfo { get; set; }


		public string serializedDistributorsInfo { get; set; }


		public string serializedLevelsInfo { get; set; }


		public string serializedReviews { get; set; }


		public string serailizedSalesConfigs { get; set; }


		public string serializedPricingInfo { get; set; }


		public string serializedSerisInfo { get; set; }


		public string serializedSubAreasInfo { get; set; }


		public string serializedSubjectsInfo { get; set; }


		public string serializedThemaCodes { get; set; }


		public string serializedBundledProducts { get; set; }


		public string serializedProductUrlInfo { get; set; }


		public string serializedExtendedPurchaseOption { get; set; }


		public string serializedProductFreeMaterial { get; set; }


		public string[] phoneticSearch { get; set; }


		public string[] exactMatch { get; set; }


		public string[] substringField { get; set; }


		public string[] themaExactmatch { get; set; }


		public string[] themaSubstring { get; set; }

		/// <summary>
		/// Index only field, not for retrieving purposes.
		/// </summary> 

		public string[] genres { get; set; }

		/// <summary>
		/// Index only field, not for retrieving purposes.
		/// </summary> 

		public string[] categories { get; set; }

		/// <summary>
		/// Index only field, not for retrieving purposes.
		/// </summary> 

		public string[] subcategories { get; set; }


		public string serializedGenres { get; set; }


		public string serializedCategories { get; set; }


		public string serializedSubcategories { get; set; }


		public string[] labels { get; set; }


		public Money defaultPrice { get; set; }


		public Money discountedPrice { get; set; }


		public string[] authorIdWithName { get; set; }


		public string extraData { get; set; }


		public string imprint { get; set; }


		public string materialWithMediaTypeName { get; set; }

		public DateTime? updateDueOn { get; set; }
	}
}

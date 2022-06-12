
namespace Gyldendal.Porter.Common.Configurations
{
    public class AppConfigurations
    {
        static AppConfigurations()
        {
            Configuration = new AppConfigurations();
        }

        public const string DanishTimeZoneId = "Romance Standard Time";

        public const string ProductStockUpdateJob = "ProductStockUpdateJob";

        public const string MediaMaterialTypeTaxonomyUpdateJob = "MediaMaterialTypeTaxonomyUpdateJob";
        
        public const string SubjectCodeTaxonomyUpdateJob = "SubjectCodeTaxonomyUpdateJob";

        public const string InternetCategoryTaxonomyUpdateJob = "InternetCategoryTaxonomyUpdateJob";
        
        public const string ImprintTaxonomyUpdateJob = "ImprintTaxonomyUpdateJob";

        public const string SupplyAvailabilityCodeTaxonomyUpdateJob = "SupplyAvailabilityCodeTaxonomyUpdateJob";
        
        public const string EducationSubjectLevelTaxonomyUpdateJob = "EducationSubjectLevelTaxonomyUpdateJob";

        public const string EntityUpdateProcessingJob = "EntityUpdateProcessingJob";


        public static AppConfigurations Configuration { get; set; }

        public HangFireConfig HangFireConfig { get; set; }

        public LoggingManagerConfig LoggingManagerConfig { get; set; }

        public MongoDbConfig MongoDbConfig { get; set; }

        public DatahubApiConfig DatahubApiConfig { get; set; }

        public AppSettings AppSettings { get; set; }

        public GpmConfig GpmConfig { get; set; }
    }
}

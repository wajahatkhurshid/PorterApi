namespace Gyldendal.Porter.Common.Configurations
{
    public class AppSettings
    {
        public string SolrShopIds { get; set; }
        
        public string SolrUrl { get; set; }

        public int EntityProcessingDegreeOfParallelism { get; set; }

        public bool UseAllAvailableProcessors { get; set; }

        public int WorkerCount { get; set; }
    }
}

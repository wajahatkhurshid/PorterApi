namespace Gyldendal.Porter.Common.Request
{
    public class SearchContributorRequest
    {
        public string SearchString { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string PropertiesToInclude { get; set; }
    }
}

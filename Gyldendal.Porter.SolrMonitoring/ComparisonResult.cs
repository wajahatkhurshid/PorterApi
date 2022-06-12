using System.Collections.Generic;

namespace Gyldendal.Porter.SolrMonitoring
{
    public class ComparisonResult
    {
        public CoreComparisonResult CoreComparison { get; set; }

        public List<ShopComparisonResult> ShopsComparison { get; set; }

    }

    public class CoreComparisonResult
    {
        public string Name { get; set; }
        public int OriginalCoreCount { get; set; }
        public int ShadowCoreCount { get; set; }
        public int Difference { get; set; }
    }
    public class ShopComparisonResult
    {
        public string Name { get; set; }
        public int OriginalCoreCount { get; set; }
        public int ShadowCoreCount { get; set; }
        public int Difference { get; set; }
        public string NotFoundInShadow { get; set; }
        public string NotFoundInOriginal { get; set; }
        public List<PropertyDifference> ItemsDifference { get; set; }

    }
    public class PropertyDifference
    {
        public string Key { get; set; }
        public Dictionary<string, string> PropertiesDifferentInOriginal { get; set; }
        public Dictionary<string, string> PropertiesDifferentInShadow { get; set; }
    }
}


using System.Collections.Generic;

namespace Gyldendal.Porter.SolrMonitoring.Product.Models
{
    public class ProductResponse
    {
        public int numFound { get; set; }
        public List<Product> docs { get; set; }
    }

    public class SolrProductQueryResponseBody
    {
        public ProductResponse response { get; set; }
    }
}

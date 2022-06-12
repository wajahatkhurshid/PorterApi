using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetProductDetailsRequest
    {
        public WebShop WebShop { get; set; }
        public string Isbn { get; set; }
        public ProductType ProductType { get; set; }
    }
}


using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetWorkByProductRequest
    {
        public ProductType ProductType { get; set; }
        public string Isbn { get; set; }
        public WebShop WebShop { get; set; }
    }
}


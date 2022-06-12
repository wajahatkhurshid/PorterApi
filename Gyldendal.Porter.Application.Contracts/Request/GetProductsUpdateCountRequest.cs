using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetProductsUpdateCountRequest
    {
        public WebShop WebShop { get; set; }
        public long UpdatedAfterTicks { get; set; }
    }
}


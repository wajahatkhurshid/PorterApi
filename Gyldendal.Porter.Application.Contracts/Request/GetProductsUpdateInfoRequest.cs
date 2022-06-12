using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetProductsUpdateInfoRequest
    {
        public WebShop WebShop { get; set; }
        public long UpdatedAfterTicks { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}

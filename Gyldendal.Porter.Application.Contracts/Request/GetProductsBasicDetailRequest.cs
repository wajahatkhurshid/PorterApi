using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetProductsBasicDetailRequest
    {
        public WebShop WebShop { get; set; }
        public List<string> Isbns { get; set; }
        public bool WithImageUrl { get; set; }
    }
}


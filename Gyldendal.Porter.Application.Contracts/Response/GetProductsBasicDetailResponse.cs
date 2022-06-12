using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Contracts.Response
{
   public class GetProductsBasicDetailResponse
    {
        public List<ProductBasicDetail> ProductBasicDetails { get; set; }
    }
}

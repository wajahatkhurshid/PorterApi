using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Contracts.Response
{
    public class GetProductsUpdateInfoResponse
    {
        public IList<ProductUpdateInfo> ProductUpdateInfos { get; set; }
    }
}

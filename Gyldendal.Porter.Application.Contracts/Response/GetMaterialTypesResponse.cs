using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Contracts.Response
{
   public class GetMaterialTypesResponse
    {
        public List<MaterialType> MaterialTypes { get; set; }
    }
    
}

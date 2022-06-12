using System.Collections.Generic;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Contracts.Response
{
   public class GetMediaTypesResponse
    {
        public List<MediaType> MediaTypes { get; set; }
    }
}

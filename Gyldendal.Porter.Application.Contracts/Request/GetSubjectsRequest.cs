using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetSubjectsRequest
    {
        public WebShop WebShop { get; set; }
        public int? areaId { get; set; }
    }
}


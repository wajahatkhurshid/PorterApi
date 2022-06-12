using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetLevelsRequest
    {
        public WebShop WebShop { get; set; }
        public int? AreaId { get; set; }
    }
}


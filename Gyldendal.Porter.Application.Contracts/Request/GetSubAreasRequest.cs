using Gyldendal.Porter.Application.Contracts.Enums;

namespace Gyldendal.Porter.Application.Contracts.Request
{
    public class GetSubAreasRequest
    {
        public WebShop WebShop { get; set; }
        public int? SubjectId { get; set; }
    }
}


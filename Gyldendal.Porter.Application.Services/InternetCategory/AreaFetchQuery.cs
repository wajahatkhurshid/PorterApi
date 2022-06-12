using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using MediatR;

namespace Gyldendal.Porter.Application.Services.InternetCategory
{
    public class AreaFetchQuery : IRequest<GetAreasResponse>
    {
        public WebShop WebShop { get; set; }

        public AreaFetchQuery(WebShop webShop)
        {
            WebShop = webShop;
        }
    }
}

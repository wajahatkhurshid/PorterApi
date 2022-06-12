using Gyldendal.Porter.Application.Contracts.Request;
using Gyldendal.Porter.Application.Contracts.Response;
using MediatR;

namespace Gyldendal.Porter.Application.Services.InternetCategory
{
    public class SubAreaFetchQuery : IRequest<GetSubAreasResponse>
    {
        public GetSubAreasRequest SubAreasRequest { get; set; }

        public SubAreaFetchQuery(GetSubAreasRequest subAreasRequest)
        {
            SubAreasRequest = subAreasRequest;
        }
    }
}

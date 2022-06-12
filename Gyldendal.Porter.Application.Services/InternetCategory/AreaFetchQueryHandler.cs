using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Models;

namespace Gyldendal.Porter.Application.Services.InternetCategory
{
    public class AreaFetchQueryHandler : IRequestHandler<AreaFetchQuery, GetAreasResponse>
    {
        private readonly IInternetCategoryRepository _internetCategoryRepository;

        public AreaFetchQueryHandler(IInternetCategoryRepository internetCategoryRepository)
        {
            _internetCategoryRepository = internetCategoryRepository;
        }

        public async Task<GetAreasResponse> Handle(AreaFetchQuery request, CancellationToken cancellationToken)
        {
            var internetCategories = await _internetCategoryRepository.GetAreasAsync(request.WebShop);
            return GetAreas(internetCategories, request.WebShop);
        }

        public GetAreasResponse GetAreas(List<Domain.Contracts.Entities.MasterData.InternetCategory> internetCategories, WebShop webShop)
        {
            GetAreasResponse areaResponse = new GetAreasResponse();
            areaResponse.Areas = new List<Area>();

            foreach(var area in internetCategories)
             areaResponse.Areas.Add(new Area
                {
                    Id = int.Parse(area.Id),
                    Name = area.Name,
                    WebShop = webShop
             });

            return areaResponse;
        }
    }
}

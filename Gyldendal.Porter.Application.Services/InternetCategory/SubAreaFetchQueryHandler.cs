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
    public class SubAreaFetchQueryHandler : IRequestHandler<SubAreaFetchQuery, GetSubAreasResponse>
    {
        private readonly IInternetCategoryRepository _internetCategoryRepository;

        public SubAreaFetchQueryHandler(IInternetCategoryRepository internetCategoryRepository)
        {
            _internetCategoryRepository = internetCategoryRepository;
        }

        public async Task<GetSubAreasResponse> Handle(SubAreaFetchQuery request, CancellationToken cancellationToken)
        {
            var internetCategories = await _internetCategoryRepository.GetSubAreasAsync(request.SubAreasRequest.WebShop,request.SubAreasRequest.SubjectId);
            return GetSubAreas(internetCategories, request.SubAreasRequest.WebShop);
        }

        public GetSubAreasResponse GetSubAreas(List<Domain.Contracts.Entities.MasterData.InternetCategory> internetCategories, WebShop webShop)
        {
            GetSubAreasResponse subAreasResponse = new GetSubAreasResponse();
            subAreasResponse.SubAreas = new List<SubArea>();

            foreach(var subArea in internetCategories)
             subAreasResponse.SubAreas.Add(new SubArea
                {
                    Id = int.Parse(subArea.Id),
                    Name = subArea.Name,
                    WebShop = webShop,
                    SubjectId = int.Parse(subArea.Parent.Id)
             });

            return subAreasResponse;
        }
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.MediaMaterialType
{
    public class MediaMaterialTypeFetchQueryHandler : IRequestHandler<MaterialTypeFetchQuery, GetMaterialTypesResponse>
    {
        private readonly IMediaMaterialTypeRepository _mediaMaterialTypeRepository;

        public MediaMaterialTypeFetchQueryHandler(IMediaMaterialTypeRepository mediaMaterialTypeRepository)
        {
            _mediaMaterialTypeRepository = mediaMaterialTypeRepository;
        }

        public async Task<GetMaterialTypesResponse> Handle(MaterialTypeFetchQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediaMaterialTypeRepository.GetMediaMaterialTypesAsync();
            return GetMaterialTypes(result);
        }

        private GetMaterialTypesResponse GetMaterialTypes(
            List<Domain.Contracts.Entities.MasterData.MediaMaterialType> mediaMaterialTypes)
        {
            var materialTypesResponse = new GetMaterialTypesResponse();
            var materialTypes = new List<MaterialType>();

            foreach (var materialType in mediaMaterialTypes)
                if (materialType.Level == 1) materialTypes.Add(new MaterialType() { Name = materialType.Name });

            materialTypesResponse.MaterialTypes = materialTypes;

            return materialTypesResponse;
        }
    }
}
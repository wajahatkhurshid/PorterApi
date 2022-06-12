using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.MediaMaterialType
{
    public class MediaTypeFetchQueryHandler : IRequestHandler<MediaTypeFetchQuery, GetMediaTypesResponse>
    {
        private readonly IMediaMaterialTypeRepository _mediaMaterialTypeRepository;

        public MediaTypeFetchQueryHandler(IMediaMaterialTypeRepository mediaMaterialTypeRepository)
        {
            _mediaMaterialTypeRepository = mediaMaterialTypeRepository;
        }

        public async Task<GetMediaTypesResponse> Handle(MediaTypeFetchQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediaMaterialTypeRepository.GetMediaMaterialTypesAsync();
            return GetMediaTypes(result);
        }

        private GetMediaTypesResponse GetMediaTypes(
            List<Domain.Contracts.Entities.MasterData.MediaMaterialType> mediaMaterialTypes)
        {
            var mediaTypesResponse = new GetMediaTypesResponse();
            var mediaTypes = new List<MediaType>();

            foreach (var mediaType in mediaMaterialTypes)
                if (mediaType.Level == 0) mediaTypes.Add(new MediaType() { Name = mediaType.Name });

            mediaTypesResponse.MediaTypes = mediaTypes;

            return mediaTypesResponse;
        }
    }
}
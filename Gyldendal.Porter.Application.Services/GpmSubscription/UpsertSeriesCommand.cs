using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using MediatR;

namespace Gyldendal.Porter.Application.Services.GpmSubscription
{
    public class UpsertSeriesCommand : IRequest<bool>
    {
        public ProductCollectionTitleContainer SeriesContainer { get; set; }

        public class UpsertSeriesCommandHandler : IRequestHandler<UpsertSeriesCommand, bool>
        {
            private readonly ISeriesRepository _seriesRepository;
            private readonly IMapper _mapper;

            public UpsertSeriesCommandHandler(ISeriesRepository seriesRepository, IMapper mapper)
            {
                _seriesRepository = seriesRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpsertSeriesCommand request, CancellationToken cancellationToken)
            {
                await _seriesRepository.UpsertSeriesAsync(_mapper.Map<Series>(request.SeriesContainer));
                return true;
            }
        }
    }
}
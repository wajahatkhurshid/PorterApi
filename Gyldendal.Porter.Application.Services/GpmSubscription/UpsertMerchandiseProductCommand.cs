using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using MediatR;

namespace Gyldendal.Porter.Application.Services.GpmSubscription
{
    public class UpsertMerchandiseProductCommand : IRequest<bool>
    {
        public MerchandiseProductContainer MerchandiseProductContainer { get; set; }

        public class UpsertMerchandiseProductCommandHandler : IRequestHandler<UpsertMerchandiseProductCommand, bool>
        {
            private readonly IMerchandiseProductRepository _merchandiseProductRepository;
            private readonly IMapper _mapper;

            public UpsertMerchandiseProductCommandHandler(IMerchandiseProductRepository merchandiseProductRepository, IMapper mapper)
            {
                _merchandiseProductRepository = merchandiseProductRepository;
                _mapper = mapper;
            }

            public async Task<bool> Handle(UpsertMerchandiseProductCommand request, CancellationToken cancellationToken)
            {
                var merchandiseProduct = _mapper.Map<Domain.Contracts.Entities.MerchandiseProduct>(request.MerchandiseProductContainer);
                merchandiseProduct.Id = merchandiseProduct.MerchandiseEan;

                await _merchandiseProductRepository.UpsertAsync(merchandiseProduct);

                return true;
            }
        }
    }
}

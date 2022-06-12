using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Models;
using Gyldendal.Porter.Domain.Contracts.Interfaces;
using MediatR;

namespace Gyldendal.Porter.Application.Services.GpmSubscription
{
    public class GetGpmSubscriptionCommand : IRequest<List<Subscription>>
    {
        public class GetGpmSubscriptionCommandHandler : IRequestHandler<GetGpmSubscriptionCommand, List<Subscription>>
        {
            private readonly ISubscriptionService _subscriptionService;
            private readonly IMapper _mapper;

            public GetGpmSubscriptionCommandHandler(ISubscriptionService subscriptionService, IMapper mapper)
            {
                _subscriptionService = subscriptionService;
                _mapper = mapper;
            }

            public async Task<List<Subscription>> Handle(GetGpmSubscriptionCommand request, CancellationToken cancellationToken)
            {
                var domainSubscriptions = await _subscriptionService.GetSubscriptions();
                var subscriptions = domainSubscriptions.Select(subscription => _mapper.Map<Subscription>(subscription)).ToList();
                return subscriptions;
            }
        }
    }
}
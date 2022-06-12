using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Common.Response;
using Gyldendal.Porter.Domain.Contracts.Interfaces;
using MediatR;

namespace Gyldendal.Porter.Application.Services.GpmSubscription
{
    public class CreateGpmSubscriptionCommand : IRequest<GpmSubscriptionResponse>
    {
        public class CreateSubscriptionCommandHandler : IRequestHandler<CreateGpmSubscriptionCommand, GpmSubscriptionResponse>
        {
            private readonly ISubscriptionService _subscriptionService;

            public CreateSubscriptionCommandHandler(ISubscriptionService subscriptionService)
            {
                _subscriptionService = subscriptionService;
            }

            public async Task<GpmSubscriptionResponse> Handle(CreateGpmSubscriptionCommand request, CancellationToken cancellationToken)
            {
                return await _subscriptionService.Create();
            }
        }
    }
}
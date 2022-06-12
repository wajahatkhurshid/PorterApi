using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Interfaces;
using MediatR;

namespace Gyldendal.Porter.Application.Services.GpmSubscription
{
    public class TriggerReplayCommand : IRequest<bool>
    {
        public int SubscriptionId { get; set; }
        public bool ShouldWipeCollections { get; set; }

        public class TriggerReplayCommandHandler : IRequestHandler<TriggerReplayCommand, bool>
        {
            private readonly IReplayService _replayService;

            public TriggerReplayCommandHandler(IReplayService replayService)
            {
                _replayService = replayService;
            }

            public async Task<bool> Handle(TriggerReplayCommand request, CancellationToken cancellationToken)
            {
                return await _replayService.TriggerReplayAsync(request.SubscriptionId, request.ShouldWipeCollections);
            }
        }
    }
}
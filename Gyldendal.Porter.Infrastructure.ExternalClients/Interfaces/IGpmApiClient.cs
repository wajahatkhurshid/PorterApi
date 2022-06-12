using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Infrastructure.ExternalClients.Gpm;

namespace Gyldendal.Porter.Infrastructure.ExternalClients.Interfaces
{
    public interface IGpmApiClient
    {
        Task<TaxonomyDataOutDto> GetTaxonomyAsync(int taxonomyId, IEnumerable<int> fromNodeIds, int? numberOfLevels, CancellationToken cancellationToken);
        Task<bool> TriggerReplayAsync(int subscriptionId);
        Task<string> FetchBusinessObjectPayloadAsync(string subscriptionScopeId, string businessObjectId);
    }
}
using System.Threading.Tasks;

namespace Gyldendal.Porter.Domain.Contracts.Interfaces
{
    public interface IReplayService
    {
        Task<bool> TriggerReplayAsync(int subscriptionId, bool shouldWipeCollections);
    }
}

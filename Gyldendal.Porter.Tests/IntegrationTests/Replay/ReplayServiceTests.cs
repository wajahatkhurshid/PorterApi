using System.Threading.Tasks;
using Gyldendal.Porter.Infrastructure.Services;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Replay
{
    public class ReplayServiceTests
    {
        [Fact(Skip = "Will receive new replay endpoint")]
        public async Task ReplayService_TriggerReplayAsync_ShouldClearCollections()
        {
            var context = IntegrationTestHelper.CreateNewMongoDbContext();
            var client = IntegrationTestHelper.CreateNewGpmApiClient(null);
            var replayService = new ReplayService(client, context);

            await replayService.TriggerReplayAsync(999999, true);
        }
    }
}

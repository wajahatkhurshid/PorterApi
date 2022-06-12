using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Services.SupplyAvailabilityCode;
using Gyldendal.Porter.Infrastructure.Repository;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Taxonomy
{
    public class SupplyAvailabilityCodeUpdateHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task SupplyAvailabilityCodeUpdateHandler_Handle_ShouldSaveData()
        {
            var repository =
                new SupplyAvailabilityCodeRepository(IntegrationTestHelper.CreateNewMongoDbContext());
            var taxonomyRepository = new TaxonomyRepository(IntegrationTestHelper.CreateNewGpmApiClient());
            var handler = new SupplyAvailabilityCodeUpdateHandler(taxonomyRepository, repository);

            await repository.DeleteAllAsync();

            var emptyCount = await repository.GetAllAsync();
            emptyCount.Should().BeEmpty("Because everything was just deleted");

            await handler.Handle(new SupplyAvailabilityCodeUpdateCommand(), CancellationToken.None);

            var newlyAdded = await repository.GetAllAsync();
            newlyAdded.Should().NotBeEmpty("A full reload has happened");
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Services.MediaMaterialType;
using Gyldendal.Porter.Infrastructure.Repository;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Taxonomy
{
    public class MediaMaterialTypeUpdateHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task MediaMaterialTypeUpdateHandler_Handle_ShouldSaveData()
        {
            var repository =
                new MediaMaterialTypeRepository(IntegrationTestHelper.CreateNewMongoDbContext(), null);
            var taxonomyRepository = new TaxonomyRepository(IntegrationTestHelper.CreateNewGpmApiClient());
            var handler = new MediaMaterialTypeUpdateHandler(repository, taxonomyRepository);

            var existing = await repository.GetAllAsync();
            foreach (var mediaMaterialType in existing)
            {
                await repository.DeleteAsync(mediaMaterialType);
            }

            var emptyCount = await repository.GetAllAsync();
            emptyCount.Should().BeEmpty("Because everything was just deleted");

            await handler.Handle(new MediaMaterialTypeUpdateCommand(), CancellationToken.None);

            var newlyAdded = await repository.GetAllAsync();
            newlyAdded.Should().NotBeEmpty("A full reload has happened");
        }
    }
}

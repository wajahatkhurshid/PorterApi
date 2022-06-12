using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Services.InternetCategory;
using Gyldendal.Porter.Infrastructure.Repository;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Taxonomy
{
    public class InternetCategoryUpdateHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task InternetCategoryUpdateHandler_Handle_ShouldSaveData()
        {
            var repository = new InternetCategoryRepository(IntegrationTestHelper.CreateNewMongoDbContext());
            var taxonomyRepository = new TaxonomyRepository(IntegrationTestHelper.CreateNewGpmApiClient());
            var handler = new InternetCategoryUpdateHandler(taxonomyRepository, repository);

            await repository.DeleteAllAsync();

            var emptyCount = await repository.GetAllAsync();
            emptyCount.Should().BeEmpty("Because everything was just deleted");

            await handler.Handle(new InternetCategoryUpdateCommand(), CancellationToken.None);

            var newlyAdded = await repository.GetAllAsync();
            newlyAdded.Should().NotBeEmpty("A full reload has happened");
        }
    }
}

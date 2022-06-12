using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Services.SubjectCode;
using Gyldendal.Porter.Infrastructure.Repository;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Taxonomy
{
   public class SubjectCodeUpdateHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task SubjectCodeUpdateHandler_Handle_ShouldSaveData()
        {
            var repository =
                new SubjectCodeRepository(IntegrationTestHelper.CreateNewMongoDbContext(), new MemoryCache(new MemoryCacheOptions()));
            var taxonomyRepository = new TaxonomyRepository(IntegrationTestHelper.CreateNewGpmApiClient());
            var handler = new SubjectCodeUpdateHandler(taxonomyRepository,repository);

            await repository.DeleteAllAsync();

            var emptyCount = await repository.GetAllAsync();
            emptyCount.Should().BeEmpty("Because everything was just deleted");

            await handler.Handle(new SubjectCodeUpdateCommand(), CancellationToken.None);

            var newlyAdded = await repository.GetAllAsync();
            newlyAdded.Should().NotBeEmpty("A full reload has happened");
        }
    }
}

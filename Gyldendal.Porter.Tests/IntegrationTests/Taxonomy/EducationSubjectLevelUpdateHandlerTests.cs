using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Application.Services.EducationSubjectLevel;
using Gyldendal.Porter.Infrastructure.Repository;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Taxonomy
{
    public class EducationSubjectLevelUpdateHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task EducationSubjectLevelUpdateHandler_Handle_ShouldSaveData()
        {
            var repository = new EducationSubjectLevelRepository(IntegrationTestHelper.CreateNewMongoDbContext(), null);
            var taxonomyRepository = new TaxonomyRepository(IntegrationTestHelper.CreateNewGpmApiClient());
            var handler = new EducationSubjectLevelUpdateCommandHandler(taxonomyRepository, repository);

            await repository.DeleteAllAsync();

            var emptyCount = await repository.GetAsync();
            emptyCount.Should().BeEmpty("Because everything was just deleted");

            await handler.Handle(new EducationSubjectLevelUpdateCommand(), CancellationToken.None);

            var newlyAdded = await repository.GetAsync();
            newlyAdded.Should().NotBeEmpty("A full reload has happened");
        }
    }
}

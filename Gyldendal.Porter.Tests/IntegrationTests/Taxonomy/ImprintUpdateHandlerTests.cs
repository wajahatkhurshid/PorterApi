using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Application.Services.Imprint;
using Gyldendal.Porter.Infrastructure.Repository;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Taxonomy
{
    public class ImprintUpdateHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task ImprintUpdateHandler_Handle_ShouldSaveData()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PorterMapper>();
            });

            var mapper = config.CreateMapper();
            var repository = new ImprintRepository(IntegrationTestHelper.CreateNewMongoDbContext());
            var taxonomyRepository = new TaxonomyRepository(IntegrationTestHelper.CreateNewGpmApiClient());
            var handler = new ImprintUpdateHandler(taxonomyRepository, repository);

            await repository.DeleteAllAsync();

            var emptyCount = await repository.GetAllAsync();
            emptyCount.Should().BeEmpty("Because everything was just deleted");

            await handler.Handle(new ImprintUpdateCommand(), CancellationToken.None);

            var newlyAdded = await repository.GetAllAsync();
            newlyAdded.Should().NotBeEmpty("A full reload has happened");
        }
    }
}

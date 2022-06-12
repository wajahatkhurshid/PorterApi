
using System.Threading.Tasks;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Xunit;
using FluentAssertions;


namespace Gyldendal.Porter.Tests.IntegrationTests
{
    public class GpmIntegrationTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task GetTaxonomyById_WithImprintTaxonomyId_FetchesImprintTaxonomy()
        {
            var taxonomyResponseRepository =
                new TaxonomyResponseRepository(IntegrationTestHelper.CreateNewMongoDbContext(), IntegrationTestHelper.CreateMapper());
            var gpmApiClient = IntegrationTestHelper.CreateNewGpmApiClient(taxonomyResponseRepository);
            var repository = new TaxonomyRepository(gpmApiClient);

            var result = await repository.GetTaxonomyByIdAsync(5);

            result.Should().NotBeNull();
            result.TaxonomyNodes.Should().HaveCount(10);
            result.TaxonomyNodes.ForEach(x =>
            {
                x.ChildNodeIds.Should().BeEmpty("Because imprints don't have child nodes");
                x.Level.Should().Be(0, "No child nodes, so no levels beside the first");
                x.ParentNodeId.Should().BeNull("No levels so no parents");
            });
        }
    }
}

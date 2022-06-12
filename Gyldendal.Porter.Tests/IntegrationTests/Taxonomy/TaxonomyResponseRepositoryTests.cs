using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Domain.Contracts.Entities.Taxonomy;
using Gyldendal.Porter.Infrastructure.Repository.Taxonomy;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Taxonomy
{
    public class TaxonomyResponseRepositoryTests
    {

        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task UpsertTaxonomy_ValidTaxonomyResponse_ShouldBeInsertedInMongoDb()
        {
            var repository = new TaxonomyResponseRepository(IntegrationTestHelper.CreateNewMongoDbContext(), IntegrationTestHelper.CreateMapper());
            var response = new TaxonomyResponse {TaxonomyId = 3, ResponsePayload = "hello2" };

            var result = await repository.InsertTaxonomyResponseAsync(response);

            result.Should().BeTrue();
            var savedTaxonomy = await repository.FindTaxonomyResponseByTaxonomyIdAsync(3);
            savedTaxonomy.TaxonomyId.Should().Be(3);
        }
    }
}
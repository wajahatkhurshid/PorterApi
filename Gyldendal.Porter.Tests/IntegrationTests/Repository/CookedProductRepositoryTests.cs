using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Api.Configuration;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Infrastructure.Repository;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Repository
{
    public class CookedProductRepositoryTests
    {
        [Fact]
        public async Task CookedProductRepository_WithValidProduct_ShouldSaveAndFindItAgain()
        {
            MongoClassMapConfiguration.UsePorterMongoClassMaps();

            var repository = new CookedProductRepository(IntegrationTestHelper.CreateNewMongoDbContext());
            await repository.DeleteAllAsync();

            var product = new CookedProduct {Id = "CookedProductId", Title = "TestTitle"};
            await repository.UpsertProduct(product);

            var productFound = await repository.FindById(product.Id);

            productFound.Should().NotBeNull();
            productFound.Title.Should().Be("TestTitle");
            productFound.Id.Should().Be("CookedProductId");
        }
    }
}
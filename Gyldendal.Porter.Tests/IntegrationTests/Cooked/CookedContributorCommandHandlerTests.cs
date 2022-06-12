using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Application.Services.Contributor;
using Gyldendal.Porter.Application.Services.Product;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using Gyldendal.Porter.Infrastructure.Repository;
using Moq;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Cooked
{
    public class CookedContributorCommandHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task CookContributor()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<PorterMapper>(); });

            var mapper = config.CreateMapper();
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(s => s.GetProductsByContributorIdAsync(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(() =>
                {
                    var product = new Product();
                    product.Isbn = "123456789";
                    product.Title = "I'm a test title";
                    product.Id = "20";
                    product.ContainerInstanceId = 20;
                    product.Websites = new List<List<GpmNode>>();
                    product.ContributorAuthors = new List<ProfileContributorAuthorContainer>()
                    {
                        new ProfileContributorAuthorContainer
                        {
                            Phase = "test",
                            ProfileImageUrl = "test",
                            ContainerInstanceId = 123,
                            PhaseState = "test",
                            ProfileID = "123",
                            ProfileName = "",
                            ProfileContributorMembers = new List<ProfileContributorMember>
                            {
                                new ProfileContributorMember
                                {
                                    ProfileMemberFirstName = "test",
                                    ProfileMemberSecondName = "test",
                                    ProfileMemberDisplayName = "test"
                                }
                            }
                        }
                    };
                    return new List<Product> {product};
                });

            var merchandiseProductRepository = new Mock<IMerchandiseProductRepository>();
            merchandiseProductRepository.Setup(r => r.GetMerchandiseProductsByContributorIdAsync(It.IsAny<int>(),
                It.IsAny<string>())).ReturnsAsync(() => new List<MerchandiseProduct>());

            var commonProductDataService = new CommonProductDataService(productRepository.Object, merchandiseProductRepository.Object);

            var cookedContributorRepository =
                new CookedContributorRepository(IntegrationTestHelper.CreateNewMongoDbContext());

            var handler = new ContributorCookingService(cookedContributorRepository, mapper, commonProductDataService);

            await handler.Cook(123, CancellationToken.None);

            var cookedContributor = await cookedContributorRepository.GetByIdAsync("123");

            cookedContributor.Should().NotBeNull();
        }
    }
}
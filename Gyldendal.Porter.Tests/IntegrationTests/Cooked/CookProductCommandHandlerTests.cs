using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Application.Services.Product;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using Gyldendal.Porter.Infrastructure.Repository;
using Moq;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.Cooked
{
    public class CookProductCommandHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task CookProduct()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<PorterMapper>(); });

            var mapper = config.CreateMapper();
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(s => s.GetProductByIdAsync(It.IsAny<string>())).Returns(() =>
            {
                var product = new Product();
                product.Isbn = "123456789";
                product.Title = "I'm a test title";
                product.Description = "This book is great";
                product.Id = "20";
                product.ContainerInstanceId = 20;
                product.Series = new List<ProductCollectionTitleContainer>();
                product.ContributorAuthors = new List<ProfileContributorAuthorContainer>();
                product.Websites = new List<List<GpmNode>>();
                return Task.FromResult(product);
            });

            var productStockRepository = new Mock<IProductStockRepository>();
            productStockRepository.Setup(s => s.GetProductStockByIsbnAsync(It.IsAny<string>())).Returns(() =>
            {
                var stock = new ProductStock();
                stock.AvailableStock = 10;
                return Task.FromResult(stock);
            });

            var workRepository = new Mock<IWorkRepository>();
            workRepository.Setup(s => s.GetByProductIdAsync(It.IsAny<int>())).Returns(() =>
            {
                var work = new Work();
                work.Id = "10";
                work.ContainerInstanceId = 10;
                work.ProductIds = new List<int> { 20 };
                work.WorkWebText = "This book is great";
                return Task.FromResult(work);
            });

            var cookedSeriesRepository = new Mock<ISeriesAssemblerService>();
            cookedSeriesRepository
                .Setup(csr => csr.Assemble(It.IsAny<ProductCollectionTitleContainer>(), It.IsAny<List<EducationLevel>>(),
                    It.IsAny<DateTime>()))
                .Returns(Task.FromResult(new CookedSeries()));

            var cookedProductRepository = new CookedProductRepository(IntegrationTestHelper.CreateNewMongoDbContext());
            var mediaMaterialTypeRepository = new Mock<IMediaMaterialTypeRepository>();
            mediaMaterialTypeRepository.Setup(mmt => mmt.GetMaterialTypeByIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new MediaMaterialType
                {
                    Id = "1",
                    Level = 1,
                    Name = "Media"
                }));

            var educationSubjectLevelExtractorService = new Mock<IEducationSubjectLevelsExtractorService>();
            var internetSubjectCookerService = new Mock<IInternetSubjectCookerService>();

            var handler = new ProductCookingService(
                productRepository.Object,
                productStockRepository.Object,
                workRepository.Object,
                cookedProductRepository,
                mapper,
                mediaMaterialTypeRepository.Object,
                cookedSeriesRepository.Object,
                educationSubjectLevelExtractorService.Object,
                internetSubjectCookerService.Object, null);

            await handler.Cook("123456789", CancellationToken.None);

            var cookedProduct = await cookedProductRepository.FindById("20");

            cookedProduct.Should().NotBeNull();
            cookedProduct.Description.Should().Be("This book is great");
            cookedProduct.Stock.Should().Be(10);
        }
    }
}
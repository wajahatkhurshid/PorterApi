using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Gyldendal.Porter.Application.Configuration.AutoMapper;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Application.Services.MerchandiseProduct;
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
    public class CookMerchandiseProductHandlerTests
    {
        [Fact]
        [Trait("Category", "IntegrationTest")]
        public async Task CookMerchandiseProduct()
        {
            var config = new MapperConfiguration(cfg => { cfg.AddProfile<PorterMapper>(); });

            var mapper = config.CreateMapper();
            var merchandiseProductRepository = new Mock<IMerchandiseProductRepository>();
            merchandiseProductRepository.Setup(s => s.GetByIdAsync(It.IsAny<string>())).Returns(() =>
            {
                var product = new MerchandiseProduct();
                product.MerchandiseEan = "1234";
                product.MerchandiseTitle = "I'm a test title";
                product.MerchandiseDescription = "This book is great";
                product.Id = "20";
                product.ContainerInstanceId = 20;
                product.MerchandiseCollectionTitle = new List<ProductCollectionTitleContainer>();
                product.MerchandiseContributorAuthor = new List<ProfileContributorAuthorContainer>();
                product.MerchandiseDisplayOnShops = new List<List<GpmNode>>();
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
                .Setup(csr => csr.Assemble(It.IsAny<ProductCollectionTitleContainer>(), It.IsAny<List<EducationLevel>>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(new CookedSeries()));

            var cookedMerchandiseProductRepository = new CookedProductRepository(IntegrationTestHelper.CreateNewMongoDbContext());
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

            var handler = new MerchandiseProductCookingService(
                merchandiseProductRepository.Object,
                productStockRepository.Object,
                workRepository.Object,
                cookedMerchandiseProductRepository,
                mapper,
                cookedSeriesRepository.Object,
                educationSubjectLevelExtractorService.Object,
                internetSubjectCookerService.Object, mediaMaterialTypeRepository.Object);

            await handler.Cook("1234", CancellationToken.None);

            var cookedMerchandiseProduct = await cookedMerchandiseProductRepository.GetByIdAsync("20");

            cookedMerchandiseProduct.Should().NotBeNull();
            cookedMerchandiseProduct.Description.Should().Be("This book is great");
            cookedMerchandiseProduct.Stock.Should().Be(10);
        }
    }
}

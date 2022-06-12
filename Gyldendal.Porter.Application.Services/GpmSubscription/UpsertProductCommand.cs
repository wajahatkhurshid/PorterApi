using System;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;
using MediatR;

namespace Gyldendal.Porter.Application.Services.GpmSubscription
{
    public class UpsertProductCommand : IRequest<bool>
    {
        public ProductContainer ProductContainer { get; set; }

        public class UpsertProductCommandHandler : IRequestHandler<UpsertProductCommand, bool>
        {
            private readonly IProductRepository _productRepository;

            public UpsertProductCommandHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task<bool> Handle(UpsertProductCommand request, CancellationToken cancellationToken)
            {
                await _productRepository.UpsertProductAsync(new Domain.Contracts.Entities.Product
                {
                    Id = request.ProductContainer.ProductISBN13,
                    ContainerInstanceId = request.ProductContainer.ContainerInstanceId,
                    Isbn = request.ProductContainer.ProductISBN13,
                    Title = request.ProductContainer.ProductTitle,
                    Subtitle = request.ProductContainer.ProductSubtitle,
                    Description = request.ProductContainer.ProductGyldendalShopText,
                    WebText = request.ProductContainer.ProductWebText,
                    MediaMaterialType = request.ProductContainer.ProductMediaMaterialeType,
                    WorkId = request.ProductContainer.WorkId,
                    FirstPrintPublishDate = request.ProductContainer.RelatedProductFirstEditionProductPublishedDate,
                    CurrentPrintRunPublishDate = request.ProductContainer.ProductPublishedDate,
                    ReadingSamples = request.ProductContainer.ProductGUSampleURL,
                    Edition = request.ProductContainer.ProductEdition,
                    NoOfPages = request.ProductContainer.ProductExtentTotalPageCount,
                    ExcuseCode = request.ProductContainer.ProductSupplyAvailabilityCode,
                    ProductEducationSubjectLevels = request.ProductContainer.ProductEducationSubjectLevel,
                    ProductGUInternetSubjects = request.ProductContainer.ProductGUInternetSubject,
                    ProductSubjectCodesMain = request.ProductContainer.ProductSubjectCodeMain,
                    ProductSubjectCodes = request.ProductContainer.ProductSubjectCode,
                    EditorialStaff = request.ProductContainer.ProductEditorialDivision,
                    Publisher = request.ProductContainer.RelatedProductOriginalProductPublisher,
                    DurationInMinutes = request.ProductContainer.ProductExtentDuration,
                    Stock = request.ProductContainer.Stock,
                    SeriesIds = request.ProductContainer.ProductCollectionTitleIds,
                    ContributorIds = request.ProductContainer.ProductContributorAuthorIds,
                    IsNextPrintRunPlanned = request.ProductContainer.IsNextPrintRunPlanned,
                    Url = request.ProductContainer.ProductGUProductURL,
                    Attachments = request.ProductContainer.MarketingAttachment,
                    MaterialTypeRank = request.ProductContainer.MaterialTypeRank,
                    MediaTypeRank = request.ProductContainer.MediaTypeRank,
                    Height = request.ProductContainer.ProductMeasurementHeight,
                    Width = request.ProductContainer.ProductMeasurementWidth,
                    ThicknessDepth = request.ProductContainer.ProductMeasurementThickness,
                    ReviewCopy = request.ProductContainer.ProductGUEnableInspectionCopy,
                    Websites = request.ProductContainer.ProductDisplayOnShops,
                    Imprint = request.ProductContainer.ProductImprint,
                    IndicativePrice = request.ProductContainer.ProductSupplyPriceWithoutVAT,
                    PriceWithoutVat = request.ProductContainer.PriceWithoutVat,
                    PriceWithVat = request.ProductContainer.ProductSupplyPriceWithVAT,
                    UpdatedTimestamp = DateTime.UtcNow,
                    ProductType = "SingleProduct",
                    IsDeleted = request.ProductContainer.IsDeleted
                });
                
                return true;
            }
        }
    }
}

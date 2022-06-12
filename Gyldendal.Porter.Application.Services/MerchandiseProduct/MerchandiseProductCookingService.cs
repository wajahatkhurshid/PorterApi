using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Common.Exceptions;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services.MerchandiseProduct
{
    public class MerchandiseProductCookingService : IMerchandiseProductCookingService
    {
        private readonly IMerchandiseProductRepository _merchandiseProductRepository;
        private readonly IProductStockRepository _productStockRepository;
        private readonly IWorkRepository _workRepository;
        private readonly IMapper _mapper;
        private readonly ICookedProductRepository _cookedProductRepository;
        private readonly ISeriesAssemblerService _seriesAssemblerService;
        private readonly IEducationSubjectLevelsExtractorService _educationSubjectLevelsExtractorService;
        private readonly IInternetSubjectCookerService _internetSubjectCookerService;
        private readonly IMediaMaterialTypeRepository _mediaMaterialTypeRepository;

        public MerchandiseProductCookingService(IMerchandiseProductRepository merchandiseProductRepository,
            IProductStockRepository productStockRepository, IWorkRepository workRepository,
            ICookedProductRepository cookedProductRepository, IMapper mapper,
            ISeriesAssemblerService seriesAssemblerService,
            IEducationSubjectLevelsExtractorService educationSubjectLevelsExtractorService,
            IInternetSubjectCookerService internetSubjectCookerService, IMediaMaterialTypeRepository mediaMaterialTypeRepository)
        {
            _merchandiseProductRepository = merchandiseProductRepository;
            _productStockRepository = productStockRepository;
            _workRepository = workRepository;
            _cookedProductRepository = cookedProductRepository;
            _mapper = mapper;
            _seriesAssemblerService = seriesAssemblerService;
            _educationSubjectLevelsExtractorService = educationSubjectLevelsExtractorService;
            _internetSubjectCookerService = internetSubjectCookerService;
            _mediaMaterialTypeRepository = mediaMaterialTypeRepository;
        }

        public async Task Cook(string ean, CancellationToken cancellationToken)
        {
            var merchandiseProduct = await _merchandiseProductRepository.GetByIdAsync(ean);
            if (merchandiseProduct == null)
            {
                throw new CookingException(
                    $"{Environment.MachineName} - No merchandise product found to be cooked in merchandise product base collection against merchandise product ID: {ean}");
            }

            var work = await _workRepository.GetByProductIdAsync(merchandiseProduct.ContainerInstanceId);
            if (work == null)
            {
                throw new CookingException(
                    $"{Environment.MachineName} - No Work found to be cooked in Work base collection against Merchandise Product ID: {merchandiseProduct.ContainerInstanceId} with EAN: {ean}");
            }

            var cookedMerchandiseProduct = _mapper.Map<CookedProduct>(merchandiseProduct);
            cookedMerchandiseProduct.ProductClass = ProductClass.Merchandise;
            cookedMerchandiseProduct.UpdatedTimestamp =
                DateTime.Now; // Products that are cooked during import process don't get missed in next update.
            cookedMerchandiseProduct.ProductType = ProductType.SingleProduct.ToString();

            // In case Product is not associated with any of the shop in GPM, we are adding None by default. i.e EKey products etc
            if (cookedMerchandiseProduct.WebShops == null || cookedMerchandiseProduct.WebShops.Count == 0)
            {
                cookedMerchandiseProduct.WebShops = new List<WebShop> { WebShop.None };
            }

            // Cooking Contributors
            var cookedContributors =
                _mapper.Map<List<CookedContributor>>(merchandiseProduct.MerchandiseContributorAuthor);
            cookedMerchandiseProduct.Contributors = cookedContributors.Select(c =>
            {
                c.UpdatedTimestamp = cookedMerchandiseProduct.UpdatedTimestamp;
                c.WebShops = cookedMerchandiseProduct.WebShops;
                c.ContributorTypeId = 7;
                return c;
            }).ToList();

            cookedMerchandiseProduct.WorkId = work.ContainerInstanceId;
            cookedMerchandiseProduct.WorkTitle = work.Title;
            cookedMerchandiseProduct.WorkDescription = work.WorkGyldendalShopText;

            // Cooking Media & Material type
            var materialTypeId = merchandiseProduct.MerchandiseMaterialeType?.SelectMany(w => w.Select(x => x.NodeId))
                .FirstOrDefault();
            var mediaMaterialType =
                await _mediaMaterialTypeRepository.GetMaterialTypeByIdAsync(materialTypeId.ToString());
            cookedMerchandiseProduct.MaterialType = mediaMaterialType.Name;
            cookedMerchandiseProduct.MediaType = mediaMaterialType.Parent?.Name;
            // TODO: Not media material types received on merch yet

            // Cooking Stock
            var stock = await _productStockRepository.GetProductStockByIsbnAsync(ean);
            cookedMerchandiseProduct.Stock = stock?.AvailableStock ?? 0;

            // Cooking InternetSubjects
            var cookedInternetSubjects = await _internetSubjectCookerService.Cook(
                merchandiseProduct.MerchandiseGUInternetSubject ?? new List<List<GpmNode>>(),
                merchandiseProduct.MerchandiseSubjectCode ?? new List<List<GpmNode>>());
            cookedMerchandiseProduct.Areas = cookedInternetSubjects?.Areas;
            cookedMerchandiseProduct.Subjects = cookedInternetSubjects?.Subjects;
            cookedMerchandiseProduct.SubAreas = cookedInternetSubjects?.SubAreas;
            cookedMerchandiseProduct.SubjectCodes = cookedInternetSubjects?.SubjectCodes;

            // Cooking EducationLevels
            var cookedEducationLevels =
                await _educationSubjectLevelsExtractorService.Extract(merchandiseProduct
                    .MerchandiseEducationSubjectLevel ?? new List<List<GpmNode>>());
            cookedMerchandiseProduct.ProductEducationSubjectLevels = cookedEducationLevels;

            // Cooking Series
            if (merchandiseProduct.MerchandiseCollectionTitle != null &&
                merchandiseProduct.MerchandiseCollectionTitle.Any())
            {
                var cookedSeries = await GetCookedSeries(merchandiseProduct.MerchandiseCollectionTitle,
                    cookedEducationLevels,
                    cookedMerchandiseProduct.UpdatedTimestamp);
                cookedMerchandiseProduct.Series = cookedSeries;
            }

            await _cookedProductRepository.UpsertAsync(cookedMerchandiseProduct);
        }


        private async Task<List<CookedSeries>> GetCookedSeries(List<ProductCollectionTitleContainer> series,
            List<EducationLevel> cookedEducationLevels, DateTime updatedTimeStamp)
        {
            var cookedSeries = new List<CookedSeries>();

            foreach (var serie in series)
            {
                var cookedSerie =
                    await _seriesAssemblerService.Assemble(serie, cookedEducationLevels, updatedTimeStamp);
                cookedSeries.Add(cookedSerie);
            }

            return cookedSeries;
        }
    }
}
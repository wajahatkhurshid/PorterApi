using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Common.Exceptions;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductCookingService : IProductCookingService
        {
            private readonly IProductRepository _productRepository;
            private readonly IProductStockRepository _productStockRepository;
            private readonly IWorkRepository _workRepository;
            private readonly IMapper _mapper;
            private readonly ICookedProductRepository _cookedProductRepository;
            private readonly IMediaMaterialTypeRepository _mediaMaterialTypeRepository;
            private readonly ISeriesAssemblerService _seriesAssemblerService;
            private readonly IEducationSubjectLevelsExtractorService _educationSubjectLevelsExtractorService;
            private readonly IInternetSubjectCookerService _internetSubjectCookerService;
            private readonly ILogger _logger;

            public ProductCookingService(IProductRepository productRepository,
                IProductStockRepository productStockRepository, IWorkRepository workRepository,
                ICookedProductRepository cookedProductRepository, IMapper mapper,
                IMediaMaterialTypeRepository mediaMaterialTypeRepository, ISeriesAssemblerService seriesAssemblerService,
                IEducationSubjectLevelsExtractorService educationSubjectLevelsExtractorService,
                IInternetSubjectCookerService internetSubjectCookerService, ILogger logger)
            {
                _productRepository = productRepository;
                _productStockRepository = productStockRepository;
                _workRepository = workRepository;
                _cookedProductRepository = cookedProductRepository;
                _mapper = mapper;
                _mediaMaterialTypeRepository = mediaMaterialTypeRepository;
                _seriesAssemblerService = seriesAssemblerService;
                _educationSubjectLevelsExtractorService = educationSubjectLevelsExtractorService;
                _internetSubjectCookerService = internetSubjectCookerService;
                _logger = logger;
            }

            public async Task Cook(string isbn, CancellationToken cancellationToken)
            {
                try
                {
                    _logger?.Info($"{Environment.MachineName} - Processing {isbn}");
                
                    var product = await _productRepository.GetProductByIdAsync(isbn);
                    if (product == null)
                    {
                        throw new CookingException(
                            $"{Environment.MachineName} - No Product found to be cooked in Product base collection against product ID: {isbn}");
                    }

                    var work = await _workRepository.GetByProductIdAsync(product.ContainerInstanceId);
                    if (work == null)
                    {
                        throw new CookingException(
                            $"{Environment.MachineName} - No Work found to be cooked in Work base collection against Product ID: {product.ContainerInstanceId} with ISBN: {isbn}");
                    }

                    var cookedProduct = _mapper.Map<CookedProduct>(product);
                    cookedProduct.ProductClass = ProductClass.Regular;
                    cookedProduct.UpdatedTimestamp =
                        DateTime.Now; // Products that are cooked during import process don't get missed in next update.

                    // In case Product is not associated with any of the shop in GPM, we are adding None by default. i.e EKey products etc
                    if (cookedProduct.WebShops == null || cookedProduct.WebShops.Count == 0)
                    {
                        cookedProduct.WebShops = new List<WebShop> { WebShop.None };
                    }

                    // Cooking Contributors
                    var cookedContributors = _mapper.Map<List<CookedContributor>>(product.ContributorAuthors);
                    cookedProduct.Contributors = cookedContributors.Select(c =>
                    {
                        c.UpdatedTimestamp = cookedProduct.UpdatedTimestamp;
                        c.WebShops = cookedProduct.WebShops;
                        c.ContributorTypeId = 7;
                        return c;
                    }).ToList();

                    cookedProduct.WorkId = work.ContainerInstanceId;
                    cookedProduct.WorkTitle = work.Title;
                    cookedProduct.WorkDescription = work.WorkGyldendalShopText;

                    // Cooking Media & Material type
                    var materialTypeId = product.MediaMaterialType?.SelectMany(w => w.Select(x => x.NodeId))
                        .FirstOrDefault();
                    var mediaMaterialType =
                        await _mediaMaterialTypeRepository.GetMaterialTypeByIdAsync(materialTypeId.ToString());
                    cookedProduct.MaterialType = mediaMaterialType.Name;
                    cookedProduct.MediaType = mediaMaterialType.Parent?.Name;

                    // Cooking Stock
                    var stock = await _productStockRepository.GetProductStockByIsbnAsync(isbn);
                    cookedProduct.Stock = stock?.AvailableStock ?? 0;

                    // Cooking InternetSubjects
                    var cookedInternetSubjects = await _internetSubjectCookerService.Cook(
                        product.ProductGUInternetSubjects,
                        product.ProductSubjectCodes);
                    cookedProduct.Areas = cookedInternetSubjects?.Areas;
                    cookedProduct.Subjects = cookedInternetSubjects?.Subjects;
                    cookedProduct.SubAreas = cookedInternetSubjects?.SubAreas;
                    cookedProduct.SubjectCodes = cookedInternetSubjects?.SubjectCodes;

                    // Cooking EducationLevels
                    var cookedEducationLevels =
                        await _educationSubjectLevelsExtractorService.Extract(product.ProductEducationSubjectLevels);
                    cookedProduct.ProductEducationSubjectLevels = cookedEducationLevels;

                    // Cooking Series
                    if (product.Series != null && product.Series.Any())
                    {
                        var cookedSeries = await GetCookedSeries(product.Series, cookedEducationLevels,
                            cookedProduct.UpdatedTimestamp);
                        cookedProduct.Series = cookedSeries;
                    }

                    await _cookedProductRepository.UpsertProduct(cookedProduct);
                }
                catch (CookingException cookingException)
                {
                    _logger?.Warning(cookingException.Message, isGdprSafe: true);
                    throw;
                }
                catch (Exception e)
                {
                    _logger?.Error($"{Environment.MachineName} - Failed to cook product update: {isbn}", e,
                        isGdprSafe: true);
                    throw;
                }
            }

            private async Task<List<CookedSeries>> GetCookedSeries(List<ProductCollectionTitleContainer> series,
                List<EducationLevel> cookedEducationLevels, DateTime updatedTimeStamp)
            {
                var cookedSeries = new List<CookedSeries>();

                foreach (var serie in series)
                {
                    var cookedSerie = await _seriesAssemblerService.Assemble(serie, cookedEducationLevels, updatedTimeStamp);
                    cookedSeries.Add(cookedSerie);
                }

                return cookedSeries;
            }
        }
}
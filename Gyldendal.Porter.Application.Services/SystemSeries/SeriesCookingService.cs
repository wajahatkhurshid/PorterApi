using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common.Exceptions;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Application.Services.SystemSeries
{
    public class SeriesCookingService : ISeriesCookingService
    {
        private readonly ICookedSeriesRepository _cookedSeriesRepository;
        private readonly IEducationSubjectLevelsExtractorService _educationSubjectLevelsExtractorService;
        private readonly ISeriesAssemblerService _seriesAssemblerService;
        private readonly ICommonProductDataService _commonProductDataService;

        public SeriesCookingService(ICookedSeriesRepository cookedSeriesRepository,
            ISeriesAssemblerService seriesAssemblerService,
            IEducationSubjectLevelsExtractorService educationSubjectLevelsExtractorService, ICommonProductDataService commonProductDataService)
        {
            _cookedSeriesRepository = cookedSeriesRepository;
            _seriesAssemblerService = seriesAssemblerService;
            _educationSubjectLevelsExtractorService = educationSubjectLevelsExtractorService;
            _commonProductDataService = commonProductDataService;
        }

        public async Task Cook(int seriesId, CancellationToken cancellationToken)
        {
            var seriesProducts = await _commonProductDataService.GetCommonSeriesProductsAsync(seriesId);

            if (seriesProducts == null || !seriesProducts.Any())
            {
                throw new CookingException($"{Environment.MachineName} - No Series found to be cooked in Product base collection against SeriesId: {seriesId}");
            }

            // As we have to add education levels from all the products under specified serie so getting from all the products
            var allProductsEducationLevelsForSerie = seriesProducts.Any(p => p.EducationSubjectLevels != null)
                ? seriesProducts.SelectMany(p => p.EducationSubjectLevels).ToList()
                : null;

            var cookedEducationLevels = allProductsEducationLevelsForSerie != null
                ? await _educationSubjectLevelsExtractorService.Extract(allProductsEducationLevelsForSerie)
                : null;

            // As the products are already those which are filtered against requested serieId so getting specific serie object from the first product sorted by updated timestamp
            var latestProduct = seriesProducts.OrderByDescending(p => p.UpdatedTimestamp).First();
            var serieToCook = latestProduct.Series.Single(s => s.ContainerInstanceId == seriesId);
            var cookedSeries = await _seriesAssemblerService.Assemble(serieToCook, cookedEducationLevels,
                latestProduct.UpdatedTimestamp);

            await _cookedSeriesRepository.UpsertSeries(cookedSeries);
        }
    }
}
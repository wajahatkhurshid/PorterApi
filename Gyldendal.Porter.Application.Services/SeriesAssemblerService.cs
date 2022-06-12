using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Entities.MasterData;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services
{
    public class SeriesAssemblerService : ISeriesAssemblerService
    {
        private readonly IInternetSubjectCookerService _internetSubjectCookerService;

        public SeriesAssemblerService(IInternetSubjectCookerService internetSubjectCookerService)
        {
            _internetSubjectCookerService = internetSubjectCookerService;
        }

        public async Task<CookedSeries> Assemble(ProductCollectionTitleContainer serieToCook, List<EducationLevel> cookedEducationLevels, DateTime updatedTimeStamp)
        {
            var cookedSeries = await CookSeries(serieToCook, cookedEducationLevels, updatedTimeStamp);

            return cookedSeries;
        }

        private async Task<CookedSeries> CookSeries(ProductCollectionTitleContainer seriesToCook, List<EducationLevel> cookedEducationLevels, DateTime updatedTimeStamp)
        {
            var cookedSeries = await CookSingleSerie(seriesToCook, parentSeries: null, cookedEducationLevels, updatedTimeStamp);

            if (seriesToCook.SeriesSubseries == null || !seriesToCook.SeriesSubseries.Any())
            {
                return cookedSeries;
            }

            var parentSeries = new CookedSeries
            {
                Id = cookedSeries.Id,
                ParentSerieId = cookedSeries.ParentSerieId,
                Name = cookedSeries.Name,
                Description = cookedSeries.Description,
                Url = cookedSeries.Url,
                Areas = cookedSeries.Areas,
                SubAreas = cookedSeries.SubAreas,
                EducationLevels = cookedSeries.EducationLevels,
                Subjects = cookedSeries.Subjects,
                ImageUrl = cookedSeries.ImageUrl,
                UpdatedTimestamp = cookedSeries.UpdatedTimestamp,
                WebShops = cookedSeries.WebShops
            };

            cookedSeries.ChildSeries = await CookSeries(seriesToCook.SeriesSubseries, cookedEducationLevels, parentSeries);

            return cookedSeries;
        }

        private async Task<List<CookedSeries>> CookSeries(List<ProductCollectionTitleContainer> series, List<EducationLevel> cookedEducationLevels, CookedSeries parentSeries)
        {
            var cookedSeries= new List<CookedSeries>();

            foreach (var serie in series)
            {
                var cookedSerie = await CookSingleSerie(serie, parentSeries, cookedEducationLevels, parentSeries.UpdatedTimestamp);
                cookedSeries.Add(cookedSerie);
            }

            return cookedSeries;
        }

        private async Task<CookedSeries> CookSingleSerie(ProductCollectionTitleContainer serie, CookedSeries parentSeries, List<EducationLevel> cookedEducationLevels, DateTime updatedTimeStamp)
        {
            var cookedSerie = new CookedSeries
            {
                Id = serie.ContainerInstanceId.ToString(),
                Name = serie.SeriesTitle,
                Description = serie.SeriesDescription,

                // TODO:
                //Need to confirm below flag mapping
                //cookedSerie.Url = ?
                //cookedSerie.ImageUrl = ?

                IsSystemSeries = serie.SeriesSerieSystemFlag,
                UpdatedTimestamp = updatedTimeStamp
            };

            if (parentSeries != null)
            {
                cookedSerie.ParentSerieId = Convert.ToInt32(parentSeries.Id);
                cookedSerie.ParentSeries = parentSeries;
            }

            var seriesInternetSubjects = serie.SeriesInternetSubject;
            var result = await _internetSubjectCookerService.Cook(seriesInternetSubjects, subjectCodesTaxonomy: null);

            cookedSerie.WebShops = result.WebShops;
            cookedSerie.Areas = result.Areas;
            cookedSerie.Subjects = result.Subjects;
            cookedSerie.SubAreas = result.SubAreas;
            cookedSerie.EducationLevels = cookedEducationLevels;

            return cookedSerie;
        }
    }
}

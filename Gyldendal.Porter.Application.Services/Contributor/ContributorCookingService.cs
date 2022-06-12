using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Common.Exceptions;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;

namespace Gyldendal.Porter.Application.Services.Contributor
{
    public class ContributorCookingService : IContributorCookingService
    {
        private readonly ICookedContributorRepository _cookedContributorRepository;
        private readonly ICommonProductDataService _commonProductDataService;
        private readonly IMapper _mapper;

        public ContributorCookingService(ICookedContributorRepository cookedContributorRepository, IMapper mapper, ICommonProductDataService commonProductDataService)
        {
            _cookedContributorRepository = cookedContributorRepository;
            _mapper = mapper;
            _commonProductDataService = commonProductDataService;
        }

        public async Task Cook(int contributorId, CancellationToken cancellationToken)
        {
            var commonProductDataList = await _commonProductDataService.GetCommonContributorProductsAsync(contributorId);
            if (commonProductDataList == null || !commonProductDataList.Any())
            {
                throw new CookingException(
                    $"{Environment.MachineName} - No Contributor found to be cooked in Product or Merchandise Product base collection against ContributorId: {contributorId}");
            }

            var allProductsWebsitesForContributor = commonProductDataList
                .SelectMany(p => p.Websites)
                .SelectMany(website => website.Select(node => (WebShop)Enum.Parse(typeof(WebShop), node.Name)))
                .Distinct()
                .ToList();

            var latestProduct = commonProductDataList.OrderByDescending(p => p.UpdatedTimestamp).First();
            var contributor = latestProduct.ContributorAuthors.First(wr => wr.ContainerInstanceId == contributorId);

            var cookedContributor = _mapper.Map<CookedContributor>(contributor);
            cookedContributor.UpdatedTimestamp = latestProduct.UpdatedTimestamp;
            cookedContributor.WebShops = allProductsWebsitesForContributor;
            cookedContributor.ContributorTypeId = 7;

            await _cookedContributorRepository.UpsertAsync(cookedContributor);
        }
    }
}
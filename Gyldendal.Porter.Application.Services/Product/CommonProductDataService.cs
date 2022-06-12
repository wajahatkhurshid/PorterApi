using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Services.Interfaces;
using Gyldendal.Porter.Domain.Contracts.Entities;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Domain.Contracts.ValueObjects;
using Gyldendal.Porter.Domain.Contracts.ValueObjects.Containers;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class CommonProductDataService: ICommonProductDataService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMerchandiseProductRepository _merchandiseProductRepository;

        public CommonProductDataService(IProductRepository productRepository, IMerchandiseProductRepository merchandiseProductRepository)
        {
            _productRepository = productRepository;
            _merchandiseProductRepository = merchandiseProductRepository;
        }

        public async Task<List<string>> GetCommonContributorProductIsbnsAsync(int contributorId)
        {
            var productProjection = GetProductProjectionForContributor();
            
            var products = await _productRepository.GetProductsByContributorIdAsync(contributorId,productProjection);
            return products.Select(p => p.Isbn).ToList();
        }

        public async Task<List<string>> GetCommonContributorEanProductIsbnsAsync(int contributorId)
        {
            var merchandiseProductProjection = GetEanProductProjectionForContributor();

            var merchandiseProducts = await _merchandiseProductRepository.GetMerchandiseProductsByContributorIdAsync(contributorId, merchandiseProductProjection);
            return merchandiseProducts.Select(mp => mp.MerchandiseEan).ToList();
        }

        public async Task<List<string>> GetCommonSeriesProductIsbnsAsync(int seriesId)
        {
            var productProjection = GetProductProjectionForSeries();

            var products = await _productRepository.GetProductBySeriesIdAsync(seriesId, productProjection);
            return products.Select(p => p.Isbn).ToList();
        }

        public async Task<List<string>> GetCommonSeriesEanProductIsbnsAsync(int seriesId)
        {
            var merchandiseProductProjection = GetEanProductProjectionForSeries();
            var merchandiseProducts = await _merchandiseProductRepository.GetMerchandiseProductBySeriesIdAsync(seriesId, merchandiseProductProjection);

            return merchandiseProducts.Select(mp => mp.MerchandiseEan).ToList();
        }

        public async Task<List<ContributorProduct>> GetCommonContributorProductsAsync(int contributorId)
        {
            var products = await _productRepository.GetProductsByContributorIdAsync(contributorId, GetProductProjectionForContributor());

            var merchandiseProducts =
                await _merchandiseProductRepository.GetMerchandiseProductsByContributorIdAsync(contributorId, GetEanProductProjectionForContributor());

            var commonProductDataList = new List<ContributorProduct>();
            commonProductDataList.AddRange(products.Select(p => new ContributorProduct
            {
                Websites = p.Websites ?? new List<List<GpmNode>>(),
                ContributorAuthors = p.ContributorAuthors ?? new List<ProfileContributorAuthorContainer>(),
                UpdatedTimestamp = p.UpdatedTimestamp
            }));
            commonProductDataList.AddRange(merchandiseProducts.Select(mp => new ContributorProduct
            {
                ContributorAuthors = mp.MerchandiseContributorAuthor ?? new List<ProfileContributorAuthorContainer>(),
                UpdatedTimestamp = mp.UpdatedTimestamp,
                Websites = mp.MerchandiseDisplayOnShops ?? new List<List<GpmNode>>()
            }));
            return commonProductDataList;
        }
        
        public async Task<List<SeriesProduct>> GetCommonSeriesProductsAsync(int seriesId)
        {
            var products = await _productRepository.GetProductBySeriesIdAsync(seriesId, GetProductProjectionForSeries());

            var merchandiseProducts =
                await _merchandiseProductRepository.GetMerchandiseProductBySeriesIdAsync(seriesId,
                    GetEanProductProjectionForSeries());
            var merchandiseSeriesProducts = merchandiseProducts?.Select(sp => new SeriesProduct
            {
                EducationSubjectLevels = sp.MerchandiseEducationSubjectLevel,
                UpdatedTimestamp = sp.UpdatedTimestamp,
                Series = sp.MerchandiseCollectionTitle
            }) ?? new List<SeriesProduct>();

            var regularSeriesProducts = products?.Select(sp => new SeriesProduct
            {
                EducationSubjectLevels = sp.ProductEducationSubjectLevels,
                Series = sp.Series,
                UpdatedTimestamp = sp.UpdatedTimestamp
            }) ?? new List<SeriesProduct>();

            var seriesProducts = new List<SeriesProduct>();
            seriesProducts.AddRange(merchandiseSeriesProducts);
            seriesProducts.AddRange(regularSeriesProducts);
            return seriesProducts;
        }

        private static string GetProductProjectionForSeries()
        {
            return
                $"{nameof(Domain.Contracts.Entities.Product.UpdatedTimestamp)}," +
                $"{nameof(Domain.Contracts.Entities.Product.ContainerInstanceId)}," +
                $"{nameof(Domain.Contracts.Entities.Product.ProductEducationSubjectLevels)}," +
                $"{nameof(Domain.Contracts.Entities.Product.Series)}";

        }

        private static string GetEanProductProjectionForSeries()
        {
            return
                $"{nameof(Domain.Contracts.Entities.MerchandiseProduct.UpdatedTimestamp)}," +
                $"{nameof(Domain.Contracts.Entities.MerchandiseProduct.ContainerInstanceId)}," +
                $"{nameof(Domain.Contracts.Entities.MerchandiseProduct.MerchandiseEducationSubjectLevel)}," +
                $"{nameof(Domain.Contracts.Entities.MerchandiseProduct.MerchandiseCollectionTitle)}";
        }

        private static string GetEanProductProjectionForContributor()
        {
            return
                $"{nameof(Domain.Contracts.Entities.MerchandiseProduct.MerchandiseContributorAuthor)}," +
                $"{nameof(Domain.Contracts.Entities.MerchandiseProduct.UpdatedTimestamp)}," +
                $"{nameof(Domain.Contracts.Entities.MerchandiseProduct.MerchandiseDisplayOnShops)}";
        }

        private static string GetProductProjectionForContributor()
        {
            return
                $"{nameof(Domain.Contracts.Entities.Product.ContributorAuthors)}," +
                $"{nameof(Domain.Contracts.Entities.Product.UpdatedTimestamp)}," +
                $"{nameof(Domain.Contracts.Entities.Product.Websites)}";
        }
    }
}

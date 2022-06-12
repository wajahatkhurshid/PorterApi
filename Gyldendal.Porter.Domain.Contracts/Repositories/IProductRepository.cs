using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Entities;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();

        Task<string> UpsertProductAsync(Product product);

        Task<Product> GetProductByIdAsync(string id);

        Task<List<Product>> GetListAsync(List<string> idList, WebShop webShop);

        Task<int> GetUpdatedCount(WebShop webshop, DateTime updateAfter);

        Task<List<Product>> GetProductUpdatedInfoAsync(WebShop webShop, DateTime updatedAfter, int pageIndex, int pageSize);
        
        Task<PagedResult<Product>> SearchProducts(SearchProductRequest request);

        Task<Product> GetProductByContributorIdAsync(int contributorId, string propertiesToInclude);

        Task<List<Product>> GetProductBySeriesIdAsync(int seriesId, string propertiesToProject);

        Task<List<Product>> GetProductsByContributorIdAsync(int contributorId, string propertiesToInclude);
    }
}

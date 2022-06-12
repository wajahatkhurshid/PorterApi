using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;

namespace Gyldendal.Porter.Domain.Contracts.Repositories
{
    public interface ICookedProductRepository : IDataAccessRepository<CookedProduct>
    {
        Task UpsertProduct(CookedProduct product);
        
        Task UpsertManyProducts(List<CookedProduct> product);
        
        Task<CookedProduct> GetProductByIdAsync(string id);

        Task<CookedProduct> GetProductDetailsAsync(string id, ProductType productType, WebShop webShop);

        Task<CookedProduct> GetProductDetailsAsync(string id, WebShop webShop);

        Task<int> GetUpdatedCount(WebShop webshop, DateTime updateAfter);
        
        Task<List<CookedProduct>> GetListAsync(List<string> idList, WebShop webShop);
        
        Task<PagedResult<CookedProduct>> SearchProducts(SearchProductRequest request);
        
        Task<List<CookedProduct>> GetProductUpdatedInfoAsync(WebShop webShop, DateTime updatedAfter, int pageIndex, int pageSize);
        
        Task<bool> IsProductCollectionExists();
        Task<long> GetCountByTimeRange(DateTime startDateTime, DateTime endDateTime);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Entities.Cooked;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using Gyldendal.Porter.Infrastructure.Repository.HelperExtensions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class CookedProductRepository : BaseRepository<CookedProduct>, ICookedProductRepository
    {
        public CookedProductRepository(PorterContext context) : base(context)
        {
        }

        public async Task UpsertProduct(CookedProduct product)
        {
            await UpsertAsync(product);
        }

        public Task UpsertManyProducts(List<CookedProduct> product)
        {
            throw new NotImplementedException();
        }

        public async Task<CookedProduct> FindById(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<CookedProduct> GetProductByIdAsync(string id)
        {
            var result = await SearchForAsync(x => x.Isbn == id);
            return result.FirstOrDefault();
        }

        public async Task<CookedProduct> GetProductDetailsAsync(string id, ProductType productType, WebShop webShop)
        {
            var strProductType = productType == ProductType.Bundle ? "BundleProduct" : "SingleProduct";
            var result = await SearchForAsync(x => x.Isbn == id && x.WebShops.Contains(webShop) && x.ProductType == strProductType);
            return result.FirstOrDefault();
        }

        public async Task<CookedProduct> GetProductDetailsAsync(string id, WebShop webShop)
        {
            var result = await SearchForAsync(x => x.Isbn == id && x.WebShops.Contains(webShop));
            return result.FirstOrDefault();
        }

        public async Task<int> GetUpdatedCount(WebShop webshop, DateTime updateAfter)
        {
            var response = await SearchForAsync(x =>
                x.UpdatedTimestamp >= updateAfter &&
                x.WebShops.Any(g => g == webshop));
            return response.Count;
        }

        public async Task<List<CookedProduct>> GetListAsync(List<string> idList, WebShop webShop)
        {
            var products = await SearchForAsync(x =>
                idList.Contains(x.Id) &&
                x.WebShops.Any(g => g == webShop));
            return products;
        }

        public async Task<PagedResult<CookedProduct>> SearchProducts(SearchProductRequest request)
        {
            var option = request.PropertiesToInclude.GetProjectionFilter<CookedProduct>();
            var filters = GetSearchFilter(request);
            option.Skip = (request.PageIndex - 1) * request.PageSize;
            option.Limit = request.PageSize;
            option.Sort = GetSortOption(request);
            option.Collation = new Collation("en", strength: CollationStrength.Secondary);
            var queryCursor = await Collection.FindAsync(filters,
                option);

            var products = await queryCursor.ToListAsync();

            var productCount = await Collection.CountDocumentsAsync(filters);

            var response = new PagedResult<CookedProduct>
            {
                TotalResults = productCount,
                Results = products
            };

            return response;
        }
        private static FilterDefinition<CookedProduct> GetSearchFilter(SearchProductRequest request)
        {
            var filter = Builders<CookedProduct>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(request.Isbn))
            {
                var regexFilter = "^" + request.Isbn;
                filter &= Builders<CookedProduct>.Filter.Regex(x => x.Isbn,
                    new BsonRegularExpression(new Regex(regexFilter)));
            }

            if (request.Isbns != null && request.Isbns.Any())
            {
                filter &= Builders<CookedProduct>.Filter.Where(x => request.Isbns.Contains(x.Isbn));
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                filter &= Builders<CookedProduct>.Filter.Regex(x => x.Title,
                    new BsonRegularExpression(new Regex(request.Title,
                        RegexOptions.IgnoreCase)));
            }

            if (!string.IsNullOrWhiteSpace(request.SubTitle))
            {
                filter &= Builders<CookedProduct>.Filter.Regex(x => x.Subtitle,
                    new BsonRegularExpression(new Regex(request.SubTitle,
                        RegexOptions.IgnoreCase)));
            }


            if (!string.IsNullOrWhiteSpace(request.AuthorName))
            {
                var regexFilter = "^" + request.AuthorName;
                filter &= Builders<CookedProduct>.Filter.ElemMatch(x => x.Contributors,
                    Builders<CookedContributor>.Filter.And(Builders<CookedContributor>.Filter.Regex(x => x.Name,
                        new BsonRegularExpression(new Regex(regexFilter,
                            RegexOptions.IgnoreCase)))));
            }
                
            if (!string.IsNullOrWhiteSpace(request.MediaType))
                filter &= Builders<CookedProduct>.Filter.Eq(x => x.MediaType, request.MediaType);
            if (request.DateFrom.HasValue && request.DateTo.HasValue)
                filter &= (Builders<CookedProduct>.Filter.Where(x => x.CurrentPrintRunPublishDate >= request.DateFrom && x.CurrentPrintRunPublishDate <= request.DateTo));
            if (request.WebShop != WebShop.All)
                filter &= (Builders<CookedProduct>.Filter.Where(x => x.WebShops.Contains(request.WebShop)));

            return filter;
        }
        private static SortDefinition<CookedProduct> GetSortOption(SearchProductRequest request)
        {
            switch (request.ProductSortByOptions)
            {
                case ProductSortByOptions.Isbn:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<CookedProduct>.Sort.Descending(x => x.Isbn);
                            default:
                                return Builders<CookedProduct>.Sort.Ascending(x => x.Isbn);
                        }
                    }

                case ProductSortByOptions.SubTitle:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<CookedProduct>.Sort.Descending(x => x.Subtitle);
                            default:
                                return Builders<CookedProduct>.Sort.Ascending(x => x.Subtitle);
                        }
                    }
                case ProductSortByOptions.Stock:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<CookedProduct>.Sort.Descending(x => x.Stock);
                            default:
                                return Builders<CookedProduct>.Sort.Ascending(x => x.Stock);
                        }
                    }
                case ProductSortByOptions.PublishDate:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<CookedProduct>.Sort.Descending(x => x.CurrentPrintRunPublishDate);
                            default:
                                return Builders<CookedProduct>.Sort.Ascending(x => x.CurrentPrintRunPublishDate);
                        }
                    }

                default:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<CookedProduct>.Sort.Descending(x => x.Title);
                            default:
                                return Builders<CookedProduct>.Sort.Ascending(x => x.Title);
                        }
                    }
            }
        }

        public async Task<List<CookedProduct>> GetProductUpdatedInfoAsync(WebShop webShop, DateTime updatedAfter, int pageIndex, int pageSize)
        {
            var response = await GetByPaginationAsync(
                x => x.UpdatedTimestamp >= updatedAfter &&
                     x.WebShops.Any(g => g == webShop)
                , pageSize, pageIndex);

            return response;
        }

        public async Task<bool> IsProductCollectionExists()
        {
            return await Collection.EstimatedDocumentCountAsync() > 0;
        }

        public async Task<long> GetCountByTimeRange(DateTime startDateTime, DateTime endDateTime)
        {
            var definition = new FilterDefinitionBuilder<CookedProduct>()
                .Where(s => s.UpdatedTimestamp >= startDateTime && s.UpdatedTimestamp <= endDateTime);
            return await Collection.CountDocumentsAsync(definition);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using System.Linq;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Domain.Contracts.Entities;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Infrastructure.Repository.HelperExtensions;
using MongoDB.Driver;
using MongoDB.Bson;
using Gyldendal.Porter.Application.Contracts.Response;

namespace Gyldendal.Porter.Infrastructure.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(PorterContext context) : base(context)
        {
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var products = await GetAllAsync();

            if (!products.Any())
            {
                products.Add(new Product
                {
                    Id = "Test",
                    Isbn = "7854961753489",
                    Stock = 0 // Stock
                });
            }

            return products;
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await GetByIdAsync(id);
        }

        public async Task<string> UpsertProductAsync(Product product)
        {
            return await UpsertAsync(product);
        }

        public async Task<bool> DeleteAllProductsAsync()
        {
            return await DeleteAllAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await DeleteAsync(new Product { Id = id });
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await GetAllAsync();
        }

        public async Task<List<Product>> GetListAsync(List<string> idList, WebShop webShop)
        {
            var products = await SearchForAsync(x =>
                idList.Contains(x.Id) &&
                x.Websites.SelectMany(w => w).Any(g => g.Name.Equals(GetDisplayName(webShop))));
            return products;
        }

        public async Task<int> GetUpdatedCount(WebShop webShop, DateTime updateAfter)
        {
            var response = await SearchForAsync(x =>
                x.UpdatedTimestamp >= updateAfter &&
                x.Websites.SelectMany(w => w).Any(g => g.Name.Equals(GetDisplayName(webShop))));
            return response.Count;
        }

        private string GetDisplayName(Enum enumValue)
        {
            return enumValue.GetType()?
                .GetMember(enumValue.ToString())?
                .First()?
                .GetCustomAttribute<DisplayAttribute>()?
                .Name;
        }

        public async Task<List<Product>> GetProductUpdatedInfoAsync(WebShop webShop, DateTime updatedAfter,
            int pageIndex, int pageSize)
        {
            var response = await GetByPaginationAsync(
                x => x.UpdatedTimestamp >= updatedAfter &&
                     x.Websites.SelectMany(w => w).Any(g => g.Name.Equals(GetDisplayName(webShop)))
                , pageSize, pageIndex);
            return response;
        }

        public async Task<PagedResult<Product>> SearchProducts(SearchProductRequest request)
        {
            var option = request.PropertiesToInclude.GetProjectionFilter<Product>();
            var filters = GetSearchFilter(request);
            option.Skip = (request.PageIndex - 1) * request.PageSize;
            option.Limit = request.PageSize;
            option.Sort = GetSortOption(request);
            option.Collation = new Collation("en", strength: CollationStrength.Secondary);
            var queryCursor = await Collection.FindAsync(filters,
                option);

            var products = await queryCursor.ToListAsync();

            var productCount = await Collection.CountDocumentsAsync(filters);

            var response = new PagedResult<Product>
            {
                TotalResults = productCount,
                Results = products
            };

            return response;
        }

        public async Task<List<Product>> GetProductBySeriesIdAsync(int seriesId, string propertiesToProject)
        {
            var option = propertiesToProject.GetProjectionFilter<Product>();
            option.Sort.Descending(x => x.UpdatedTimestamp);
            var products = await Collection.FindAsync(x => x.Series.Any(s => s.ContainerInstanceId == seriesId), option).Result.ToListAsync();
            return products;
        }

        private static SortDefinition<Product> GetSortOption(SearchProductRequest request)
        {
            switch (request.ProductSortByOptions)
            {
                case ProductSortByOptions.Isbn:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<Product>.Sort.Descending(x => x.Isbn);
                            default:
                                return Builders<Product>.Sort.Ascending(x => x.Isbn);
                        }
                    }

                case ProductSortByOptions.SubTitle:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<Product>.Sort.Descending(x => x.Subtitle);
                            default:
                                return Builders<Product>.Sort.Ascending(x => x.Subtitle);
                        }
                    }
                case ProductSortByOptions.Stock:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<Product>.Sort.Descending(x => x.Stock);
                            default:
                                return Builders<Product>.Sort.Ascending(x => x.Stock);
                        }
                    }
                case ProductSortByOptions.PublishDate:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<Product>.Sort.Descending(x => x.CurrentPrintRunPublishDate);
                            default:
                                return Builders<Product>.Sort.Ascending(x => x.CurrentPrintRunPublishDate);
                        }
                    }

                default:
                    {
                        switch (request.SortBy)
                        {
                            case SortBy.Desc:
                                return Builders<Product>.Sort.Descending(x => x.Title);
                            default:
                                return Builders<Product>.Sort.Ascending(x => x.Title);
                        }
                    }
            }
        }

        private static FilterDefinition<Product> GetSearchFilter(SearchProductRequest request)
        {
            var filter = Builders<Product>.Filter.Empty;

            if (!string.IsNullOrWhiteSpace(request.Isbn))
            {
                var regexFilter = "^" + request.Isbn;
                filter &= Builders<Product>.Filter.Regex(x => x.Isbn,
                    new BsonRegularExpression(new Regex(regexFilter)));
            }

            if (request.Isbns.Any())
            {
                filter &= Builders<Product>.Filter.Where(x => request.Isbns.Contains(x.Isbn));
            }

            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                filter &= Builders<Product>.Filter.Regex(x => x.Title,
                    new BsonRegularExpression(new Regex(request.Title,
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace)));
            }

            if (!string.IsNullOrWhiteSpace(request.SubTitle))
            {
                filter &= Builders<Product>.Filter.Regex(x => x.Subtitle,
                    new BsonRegularExpression(new Regex(request.SubTitle,
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace)));
            }


            if (!string.IsNullOrWhiteSpace(request.AuthorName))
                filter &= (Builders<Product>.Filter.ElemMatch(x => x.ContributorAuthors, request.AuthorName));
            if (!string.IsNullOrWhiteSpace(request.MediaType))
                filter &= Builders<Product>.Filter.ElemMatch(x => x.MediaMaterialType, request.MediaType);
            if (request.DateFrom.HasValue && request.DateTo.HasValue)
                filter &= (Builders<Product>.Filter.Where(x => x.CurrentPrintRunPublishDate >= request.DateFrom && x.CurrentPrintRunPublishDate <= request.DateTo));
            
            return filter;
        }

        public async Task<Product> GetProductByContributorIdAsync(int contributorId, string propertiesToInclude)
        {
            var option = propertiesToInclude.GetProjectionFilter<Product>();
            option.Sort.Descending(x => x.UpdatedTimestamp);
            option.Limit = 1;
            var productSearch = await Collection.FindAsync(w => w.ContributorAuthors.Any(wr => wr.ContainerInstanceId == contributorId), option);
            return await productSearch.FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetProductsByContributorIdAsync(int contributorId, string propertiesToInclude)
        {
            var option = propertiesToInclude.GetProjectionFilter<Product>();
            option.Sort.Descending(x => x.UpdatedTimestamp);
            var productSearch = await Collection.FindAsync(w => w.ContributorAuthors.Any(wr => wr.ContainerInstanceId == contributorId), option);
            return await productSearch.ToListAsync();
        }
    }
}
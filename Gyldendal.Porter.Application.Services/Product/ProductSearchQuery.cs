using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gyldendal.Porter.Application.Contracts.Enums;
using Gyldendal.Porter.Application.Contracts.Response;
using Gyldendal.Porter.Common.Enums;
using Gyldendal.Porter.Common.Request;
using Gyldendal.Porter.Domain.Contracts.Repositories;
using MediatR;

namespace Gyldendal.Porter.Application.Services.Product
{
    public class ProductSearchQuery : IRequest<SearchProductResponse>
    {
        /// <summary>
        /// Finds ISBNs that start with this value or match it exactly
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// Find products that match this list of ISBNs exactly
        /// </summary>
        public List<string> Isbns { get; set; }
        public string PropertiesToInclude { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string MediaType { get; set; }
        public WebShop WebShop { get; set; }
        public string SubTitle { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public ProductSortByOptions ProductSortByOption { get; set; }
        public SortBy SortBy { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public class GetProductQueryHandler : IRequestHandler<ProductSearchQuery, SearchProductResponse>
        {
            private readonly ICookedProductRepository _productRepository;
            private readonly IMapper _mapper;

            public GetProductQueryHandler(ICookedProductRepository productRepository, IMapper mapper)
            {
                _productRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<SearchProductResponse> Handle(ProductSearchQuery request, CancellationToken cancellationToken)
            {
                var result = await _productRepository.SearchProducts(new SearchProductRequest
                {
                    Isbn = request.Isbn,
                    Isbns = request.Isbns,
                    MediaType = request.MediaType,
                    Title = request.Title,
                    AuthorName = request.AuthorName,
                    WebShop = request.WebShop,
                    SubTitle = request.SubTitle,
                    PropertiesToInclude = request.PropertiesToInclude,
                    PageSize = request.PageSize > 0 ? request.PageSize : 100,
                    PageIndex = request.PageIndex > 0 ? request.PageIndex : 1,
                    ProductSortByOptions = request.ProductSortByOption,
                    SortBy = request.SortBy,
                    DateFrom = request.DateFrom,
                    DateTo = request.DateTo
                });

                return new SearchProductResponse
                {
                    Results = result.Results.Select(x => _mapper.Map<Contracts.Models.Product>(x)).ToList(),
                    PageSize = request.PageSize,
                    CurrentPage = request.PageIndex,
                    TotalResults = result.TotalResults
                };
            }

        }
    }
}

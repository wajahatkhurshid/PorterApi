using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Gyldendal.Porter.Application.Contracts;
using Gyldendal.Porter.Common;
using Gyldendal.Porter.Common.Exceptions;
using Gyldendal.Porter.Domain.Contracts.Interfaces;

namespace Gyldendal.Porter.Infrastructure.ExternalClients.Datahub
{
    public class DatahubProductStockClient : IProductStockClient
    {
        private readonly HttpClient _httpClient;

        public DatahubProductStockClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> FetchAvailableStockAsync(string isbn)
        {
            var response =
                await _httpClient.GetAsync($"/api/ProductStock/api/v1/Stock/GetProductStockByIsbn?isbn={isbn}");

            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException((ulong)ErrorCodes.GetStockFailure, $"{string.Format(ErrorCodes.GetStockFailure.GetDescription(), isbn, response.StatusCode)}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var productStockResponse = JsonSerializer.Deserialize<GetProductStockByIsbnResponse>(responseString, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return productStockResponse?.ProductStock?.StockAmount ?? 0;
        }

        public class GetProductStockByIsbnResponse
        {
            public ProductStockInformation ProductStock { get; set; }
        }

        public class ProductStockInformation
        {
            public string ProductId { get; set; }
            public int? StockAmount { get; set; }
            public DateTime MetaCreatedTimestamp { get; set; }
        }
    }
}
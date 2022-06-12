using System;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Gyldendal.Porter.Infrastructure.ExternalClients.Datahub;
using Xunit;

namespace Gyldendal.Porter.Tests.IntegrationTests.DatahubClient
{
    public class DatahubClientTests
    {
        [Fact(Skip = "For local testing")]
        public async Task GetStock_ValidIsbn_ShouldReturnStock()
        {
            var client = new HttpClient();
            var url = "https://localhost:44371/";
            client.BaseAddress = new Uri(url);
            var datahubClient = new DatahubProductStockClient(client);

            var stockFetchResponse = await datahubClient.FetchAvailableStockAsync("5050582874136");

            stockFetchResponse.Should().NotBe(0);
        }
    }
}

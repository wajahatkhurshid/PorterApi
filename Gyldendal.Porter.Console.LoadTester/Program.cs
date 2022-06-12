using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Gyldendal.Porter.Infrastructure.Repository;

namespace Gyldendal.Porter.Console.LoadTester
{
    public class Program
    {
        //private static string _porterUrl = "https://localhost:44372/";
        //private static string _porterUrl = "http://localhost:5200/";
        //private static string _porterUrl = "https://dev-porter.gyldendal.local/";
        private static string _porterUrl = "https://test-porter.gyldendal.local/";

        //private static PorterContext _porterContext = new PorterContext("mongodb://localhost/?maxPoolSize=6000", "PorterDb");
        //private static PorterContext _porterContext = new PorterContext("mongodb://Porter.Writer:Pxggg8yitXBppfnb@dmgdb01.gyldendal.local:27017,dmgdb02.gyldendal.local:27017,dmgdb03.gyldendal.local:27017/admin?authSource=admin&replicaSet=DMGDBCLU&readPreference=primary&appname=MongoDB%20Compass&ssl=false&minPoolSize=200&maxPoolSize=6000", "PorterDb");
        private static PorterContext _porterContext = new PorterContext("mongodb://Porter.Writer:D3Lnrzt2UpaFjXHv@tmgdb01.gyldendal.local:27017,tmgdb02.gyldendal.local:27017,tmgdb03.gyldendal.local:27017/admin?authSource=admin&replicaSet=TMGDBCLU&readPreference=primary&appname=MongoDB%20Compass&ssl=false&minPoolSize=200&maxPoolSize=6000", "PorterDb");

        public static void Main(string[] args)
        {
            var requestMessages = CreateContainerHttpRequestMessages();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var waitingTimes = SendHttpRequestMessages(requestMessages);
            stopwatch.Stop();
            var average = stopwatch.ElapsedMilliseconds / requestMessages.Count;
            System.Console.WriteLine($"Average request time: {average} milliseconds for {waitingTimes.Count} requests in {stopwatch.Elapsed:g}.");
            System.Console.ReadKey();
        }

        public static List<HttpRequestMessage> CreateContainerHttpRequestMessages()
        {
            const int productRecordsToBeGenerated = 10000;
            const int merchandiseRecordsToBeGenerated = 220;
            var productContainers = GetFakeProductContainers(productRecordsToBeGenerated);
            var workContainers = GetFakeWorkContainers(productRecordsToBeGenerated, productContainers.FakeProductIds).ToList();
            var merchandiseProductContainers = GetFakeMerchandiseProductContainers(merchandiseRecordsToBeGenerated);

            //System.IO.File.WriteAllLines("d:/FakeProductContainers.json", productContainers.RequestBodies);
            //System.IO.File.WriteAllLines("d:/FakeWorkContainers.json", workContainers);

            var productMessages = GetHttpRequestMessages(productContainers.RequestBodies, containerType: "Product");
            var workMessages = GetHttpRequestMessages(workContainers, containerType: "Work");
            var merchandiseProductMessages = GetHttpRequestMessages(merchandiseProductContainers.RequestBodies, containerType: "Merchandise");

            var requestMessages = new List<HttpRequestMessage>();
            requestMessages.AddRange(productMessages);
            requestMessages.AddRange(workMessages);
            requestMessages.AddRange(merchandiseProductMessages);

            return requestMessages;
        }

        private static (IEnumerable<string> RequestBodies, IEnumerable<int> FakeProductIds) GetFakeProductContainers(int count)
        {
            var containerFaker = new ProductContainerBuilder(_porterContext)
                .WithFakeMediaMaterialType()
                .WithFakeWebShop()
                .WithFakeSupplyAvailabilityCode()
                .WithFakeSubject()
                .WithFakeSubjectCode()
                .WithFakeSubjectLevel()
                .WithFakeInternetSubject()
                .WithFakeSubjectCodeMain()
                .WithFakeProductCollectionTitle()
                .WithFakeMarketingAttachment()
                .WithFakeContributorAuthor()
                .WithFakeProductImprint()
                .Build();

            var containers = containerFaker.Generate(count);
            var productIds = containers.Select(c => c.ContainerInstanceId);
            var requestBodies = containers.Select(x => JsonSerializer.Serialize(x));

            return (requestBodies, productIds);
        }

        private static (IEnumerable<string> RequestBodies, IEnumerable<int> FakeMerchandiseProductIds) GetFakeMerchandiseProductContainers(int count)
        {
            var containerFaker = new MerchandiseProductContainerBuilder(_porterContext)
                .WithFakeMediaMaterialType()
                .WithFakeWebShop()
                .WithFakeSupplyAvailabilityCode()
                .WithFakeSubject()
                .WithFakeSubjectCode()
                .WithFakeSubjectLevel()
                .WithFakeInternetSubject()
                .WithFakeSubjectCodeMain()
                .WithFakeProductCollectionTitle()
                .WithFakeMarketingAttachment()
                .WithFakeContributorAuthor()
                .WithFakeProductImprint()
                .Build();

            var containers = containerFaker.Generate(count);
            var productIds = containers.Select(c => c.ContainerInstanceId);
            var requestBodies = containers.Select(x => JsonSerializer.Serialize(x));

            return (requestBodies, productIds);
        }

        private static IEnumerable<string> GetFakeWorkContainers(int count, IEnumerable<int> fakeProductIds)
        {
            var containerFaker = new WorkContainerBuilder()
                .WithFakeProductIdsAs(fakeProductIds)
                .WithFakeSubjectCodeMain()
                .WithFakeSubjectCode()
                .WithFakeSubject()
                .WithFakeInternetSubject()
                .WithFakeWebShop()
                .WithFakeSubjectLevel()
                .WithFakeWorkReviews()
                .Build();

            var containers = containerFaker.Generate(count);
            var requestBodies = containers.Select(x => JsonSerializer.Serialize(x));

            return requestBodies;
        }

        private static IEnumerable<HttpRequestMessage> GetHttpRequestMessages(IEnumerable<string> containers, string containerType)
        {
            var messages = containers.Select(pc =>
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Put, new Uri("/GpmSubscription", UriKind.Relative));
                requestMessage.Headers.Add("subscriptionname", $"loadtester{containerType}");
                requestMessage.Headers.Add("scope", $"loadtester{containerType}scope");
                requestMessage.Headers.Add("businessobjectname", $"{containerType}BusinessObject");
                requestMessage.Content = new StringContent(pc, Encoding.UTF8, "application/json");

                return requestMessage;
            });

            return messages;
        }

        public static List<TimeSpan> SendHttpRequestMessages(List<HttpRequestMessage> httpRequestMessages)
        {
            var waitingTime = new List<TimeSpan>();
            var client = new HttpClient();
            client.BaseAddress = new Uri(_porterUrl);

            int percentageOfProcessorToBeUsed = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * (70 * 0.01)));

            var token = CancellationToken.None;

            var parallelismSettings = new ParallelOptions { CancellationToken = token, MaxDegreeOfParallelism = percentageOfProcessorToBeUsed };

            System.Console.WriteLine($"Sending {httpRequestMessages.Count} requests in total");

            Parallel.ForEach(httpRequestMessages, parallelOptions: parallelismSettings, body: httpRequest =>
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var response = client.SendAsync(httpRequest, token).Result;
                stopWatch.Stop();

                if (!response.IsSuccessStatusCode)
                {
                    System.Console.WriteLine("Failed to send request");
                }
                waitingTime.Add(stopWatch.Elapsed);
            });

            return waitingTime;
        }
    }
}
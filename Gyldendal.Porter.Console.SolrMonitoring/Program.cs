using System;
using System.Configuration;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Gyldendal.Porter.SolrMonitoring;
using Gyldendal.Porter.SolrMonitoring.Contributor;
using Gyldendal.Porter.SolrMonitoring.Product;
using Gyldendal.Porter.SolrMonitoring.WorkReviews;

namespace Gyldendal.Porter.Console.SolrMonitoring
{
    class Program
    {
        private static string solrUrl = ConfigurationManager.AppSettings["SolrUrl"];
        private static string shopIds = ConfigurationManager.AppSettings["SolrShopIds"];

        public static async Task Main(string[] args)
        {
            do
            {
                System.Console.WriteLine("****SOLR MONITORING CONSOLE****");
                System.Console.WriteLine("###################################");
                System.Console.WriteLine("Webshop Ids are =>  0=All Shops" +
                                         "2=GU,3=HR,5=Munks,11=ClubBogklub,12=ClubBoerne,13=ClubSamleren,14=ClubKrimi,15=ClubPsykeSjael,16=ClubHistorie,17=ClubPaedagogisk," +
                                         "18=Highlight,19=ClubBoerne3To5,20=ClubBoerne5To10,21=ClubFlamingo,22,23,24,26");
                System.Console.WriteLine("###################################");
                System.Console.WriteLine();
                System.Console.WriteLine("Modes: 1=Product, 2=Contributors, 3=WorkReviews");
                System.Console.WriteLine();
                System.Console.WriteLine("###################################");
                System.Console.WriteLine();
                var path = @"C:\SolrComparison";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                System.Console.Write("Select Mode: ");
                var mode = Convert.ToInt32(System.Console.ReadLine());
                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine("###################################");
                System.Console.WriteLine("Criteria: 1=For Shop Only , 2=For Id Only , 3=Id With Shop");
                System.Console.Write("Select Criteria: ");
                var selectCriteria = Convert.ToInt32(System.Console.ReadLine());
                var shopId = 0;
                var id = "";
                switch (selectCriteria)
                {
                    case 1:
                        System.Console.WriteLine();
                        System.Console.Write("Select WebshopId: ");
                        shopId = Convert.ToInt32(System.Console.ReadLine());
                        break;
                    case 2:
                        System.Console.WriteLine();
                        System.Console.Write("Select Id: ");
                        id = Convert.ToString(System.Console.ReadLine());
                        break;
                    case 3:
                        System.Console.WriteLine();
                        System.Console.Write("Select WebshopId: ");
                        shopId = Convert.ToInt32(System.Console.ReadLine());
                        System.Console.WriteLine();
                        System.Console.Write("Select Id: ");
                        id = Convert.ToString(System.Console.ReadLine());
                        break;
                }
                System.Console.WriteLine();
                System.Console.Write("Solr Comparison in progress....");
                switch (mode)
                {
                    case 1:
                        await CompareProducts(path, shopId, id);
                        break;
                    case 2:
                        await CompareContributors(path, shopId, id);
                        break;
                    case 3:
                        await CompareWorkReviews(path, shopId, id);
                        break;

                }

                var criteria = new SolrMonitoringRequest() { ShopId = shopId,Id = id};
                var result = await ProductMonitoringService.CompareProducts(criteria, solrUrl, shopIds);
                System.Console.WriteLine();
                System.Console.Write("Comparison file has been generated successfully! ");
                System.Console.WriteLine();
            } while (MonitorAgain());
        }

        private static bool MonitorAgain()
        {
            while (true) // Continue asking until a correct answer is given.
            {
                System.Console.Write("Do you want to monitor again [Y/N]?");
                var answer = System.Console.ReadLine()?.ToUpper();
                if (answer == "Y")
                    return true;
                if (answer == "N")
                    return false;
            }
        }
        private static async Task CompareWorkReviews(string path, int shopId,string id)
        {
            var criteria = new SolrMonitoringRequest() { ShopId = shopId , Id = id};
            var result = await WorkReviewsMonitoringService.CompareWorkReviews(criteria, solrUrl, shopIds);
            //serialize object directly into file stream
            var json = JsonSerializer.Serialize(result);

            var filePath = Path.Combine(path, $"SolrWorkReviewsMonitoring_{DateTime.Now:yyyyMMdd_hhmmss}");
            await File.WriteAllTextAsync(filePath, json);
        }

        private static async Task CompareContributors(string path, int shopId,string id)
        {
            var criteria = new SolrMonitoringRequest() { ShopId = shopId , Id = id};
            var result = await ContributorMonitoringService.CompareContributors(criteria, solrUrl, shopIds);
            //serialize object directly into file stream
            var json = JsonSerializer.Serialize(result);

            var filePath = Path.Combine(path, $"SolrContributorMonitoring_{DateTime.Now:yyyyMMdd_hhmmss}");
            await File.WriteAllTextAsync(filePath, json);
        }

        public static async Task CompareProducts(string path, int shopId,string id)
        {
            var criteria = new SolrMonitoringRequest() { ShopId = shopId ,Id = id};
            var result = await ProductMonitoringService.CompareProducts(criteria, solrUrl, shopIds);
            //serialize object directly into file stream
            var json = JsonSerializer.Serialize(result);

            var filePath = Path.Combine(path, $"SolrProductMonitoring_{DateTime.Now:yyyyMMdd_hhmmss}");
            await File.WriteAllTextAsync(filePath, json);
        }


    }


}

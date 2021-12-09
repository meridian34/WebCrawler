using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.Models;
using WebCrawler.Services;

namespace WebCrawler.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var baseUrl = new Uri("https://www.ukad-group.com/123/");
            var basePath = baseUrl.GetLeftPart(UriPartial.Authority);
            var newUri = new Uri(basePath);


            //var url = Console.ReadLine();

            var webCrawler = new WebCrawlerService();
            var result = await webCrawler.RunCrawler("https://www.ukad-group.com/");
            PrintSitemapUniqueLink(result.Where(x=>x.IsSiteMap == true && x.IsSiteScan == false));
            PrintSiteScanUniqueLink(result.Where(x => x.IsSiteMap == false && x.IsSiteScan == true));
            PrintTimeResult(result);
            PrintCount(result.Where(x => x.IsSiteScan == true).Count(), result.Where(x => x.IsSiteMap == true ).Count());
           
            Console.ReadKey();            
        }

        

        private static void PrintSitemapUniqueLink(IEnumerable<CrawlResult> results)
        {
            PrintResult(results, "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
        }

        private static void PrintSiteScanUniqueLink(IEnumerable<CrawlResult> results)
        {
            PrintResult(results, "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
        }

        private static void PrintResult(IEnumerable<CrawlResult> results, string headerMessage)
        {
            Console.WriteLine($"{headerMessage}"); 
            
            foreach (var i in results)
            {
                Console.WriteLine($"{i.Url}");
            }

            Console.WriteLine();
        }

        private static void PrintTimeResult(IEnumerable<CrawlResult> results)
        {
            Console.WriteLine($"Timing");

            foreach (var i in results)
            {
                Console.WriteLine($"{i.Url}");
                Console.WriteLine($"{i.ElapsedMilliseconds}");
            }

            Console.WriteLine();
        }

        private static void PrintCount(int countScanResults, int countSitemapResults)
        {
            Console.WriteLine($"Urls(html documents) found after crawling a website: {countScanResults}");
            Console.WriteLine($"Urls found in sitemap: {countSitemapResults}");
        }
    }
}
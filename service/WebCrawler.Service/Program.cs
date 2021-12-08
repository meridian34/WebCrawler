using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            

            //var url = Console.ReadLine();
            var webCrawler = Startup.GetWebCrawler;
            await webCrawler.RunCrawler("https://www.ukad-group.com/");
            PrintSitemapUniqueLink(webCrawler.GetSitemapUniqueResults());
            PrintSiteScanUniqueLink(webCrawler.GetScanUniqueResults());
            PrintTimeResult(webCrawler.GetAllSortedResults());
            PrintCount(webCrawler.GetScanResults().Count, webCrawler.GetSitemapResults().Count);
           
            Console.ReadKey();            
        }

        

        private static void PrintSitemapUniqueLink(IReadOnlyCollection<HttpScanResult> results)
        {
            PrintResult(results, "Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site");
        }

        private static void PrintSiteScanUniqueLink(IReadOnlyCollection<HttpScanResult> results)
        {
            PrintResult(results, "Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml");
        }

        private static void PrintResult(IReadOnlyCollection<HttpScanResult> results, string headerMessage)
        {
            Console.WriteLine($"{headerMessage}");            
            foreach (var i in results)
            {
                Console.WriteLine($"{i.Url}");
            }
            Console.WriteLine();
        }

        private static void PrintTimeResult(IReadOnlyCollection<HttpScanResult> results)
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
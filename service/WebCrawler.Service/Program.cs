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
            //string path = @"C:\Users\a.sobol\source\repos_vs_code\WebCrawler\webcrawler\service\WebCrawler.Service\test.txt";
            //string source = "";

            //try
            //{
            //    using (StreamReader sr = new StreamReader(path))
            //    {
            //        source = sr.ReadToEnd();
            //    }
                
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //var res =  source.Substring(2791, source.Length - 2791);



            //var url = Console.ReadLine();
            var webCrawler = Startup.GetWebCrawler;
            await webCrawler.RunCrawler("https://www.ukad-group.com/");
            PrintSitemapUniqueLink(webCrawler.GetSitemapUniqueResults());
            PrintSiteScanUniqueLink(webCrawler.GetScanUniqueResults());
            PrintTimeResult(webCrawler.GetAllSortedResults());
            PrintCount(webCrawler.GetScanResults().Count, webCrawler.GetSitemapResults().Count);
            Console.WriteLine();
            foreach(var item in webCrawler.GetScanResults())
            {
                Console.WriteLine(item.Url);
            }
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
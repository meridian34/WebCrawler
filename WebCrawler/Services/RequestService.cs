using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using WebCrawler.Models;

namespace WebCrawler.Services
{
    public class RequestService
    {
        public virtual IEnumerable<PerfomanceData> GetElapsedTimeForLinks(IEnumerable<Link> links)
        {
            var results = new List<PerfomanceData>();

            foreach(var link in links)
            {
                var perfomanceItem = GetElapsedTime(link.Url);
                results.Add(perfomanceItem);
            }

            return results;
        }

        public virtual PerfomanceData GetElapsedTime(Uri url)
        {
            var timer = new Stopwatch();
            var result = new PerfomanceData();
            result.Url = url;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                timer.Start();
                var response = (HttpWebResponse)(request.GetResponse());
                timer.Stop();
                result.ElapsedMilliseconds = (int)timer.ElapsedMilliseconds;
            }
            catch (WebException)
            {
                timer.Stop();
                result.ElapsedMilliseconds = (int)timer.ElapsedMilliseconds;
            }

            return result;
        }

        public virtual string Download(Uri url)
        {
            using var client = new WebClient();
            string data;
            try
            {
                data = client.DownloadString(url);
            }
            catch (WebException)
            {
                return null;
            }

            return data;
        }
    }
    
}

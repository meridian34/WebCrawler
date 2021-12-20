using System.Threading.Tasks;

namespace WebCrawler.ConsoleApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await Startup.GetWebCrawler.RunAsync();
        }
    }
}
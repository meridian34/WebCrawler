using System;

namespace WebCrawler.ConsoleApplication.Services
{
    public class ConsoleService
    {
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }

        public virtual void WriteLine(string data)
        {
            Console.WriteLine(data);
        }
    }
}

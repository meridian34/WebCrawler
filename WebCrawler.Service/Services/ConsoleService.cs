using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

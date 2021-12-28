using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using WebCrawler.EntityFramework.Entities;
using WebCrawler.WebApplication.Models;

namespace WebCrawler.WebApplication.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewData["TestData"] = "Давай Морти это приключение на 20 минут!";
            ViewBag.Message = "Five moments later...";
            return View();
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult TestData([FromQuery] int testId)
        {
            var list = new List<Link>()
            {
                new Link { Id = 1, Url = "first", FromHtml = true, FromSitemap = true, ElapsedMilliseconds = 200, TestId = 1 },
                new Link { Id = 1, Url = "second", FromHtml = true, FromSitemap = false, ElapsedMilliseconds = 200, TestId = 2 },
                new Link { Id = 1, Url = "third", FromHtml = false, FromSitemap = true, ElapsedMilliseconds = 200, TestId = 3 }
            };

            return View(list.Where(x=>x.TestId == testId));
        }

        public IActionResult TestData()
        {
            var list = new List<Link>()
            {
                new Link { Id = 1, Url = "first", FromHtml = true, FromSitemap = true, ElapsedMilliseconds = 200 },
                new Link { Id = 1, Url = "second", FromHtml = true, FromSitemap = false, ElapsedMilliseconds = 200 },
                new Link { Id = 1, Url = "third", FromHtml = false, FromSitemap = true, ElapsedMilliseconds = 200 }
            };

            return View(list);
        }

        [HttpGet("[controller]/[action]/{id}")]
        public IActionResult Tests([FromRoute] int id)
        {
            var list = new List<Test>()
            {
                new Test { Id = 1, TestDateTime = DateTimeOffset.UtcNow, UserLink = "first"},
                new Test { Id = 2, TestDateTime = DateTimeOffset.UtcNow, UserLink = "second" },
                new Test { Id = 3, TestDateTime = DateTimeOffset.UtcNow, UserLink = "third"}
            };
            var result = list.Where(x => x.Id == id);
            return View(result);
        }

        public IActionResult Tests()
        {
            var list = new List<Test>()
            {
                new Test { Id = 1, TestDateTime = DateTimeOffset.UtcNow, UserLink = "first"},
                new Test { Id = 2, TestDateTime = DateTimeOffset.UtcNow, UserLink = "second" },
                new Test { Id = 3, TestDateTime = DateTimeOffset.UtcNow, UserLink = "third"}
            };

            return View(list);
        } 

        public IActionResult Search()
        {   
            return View();
        }

       
        [HttpPost]
        public IActionResult Search(string url)
        {

            Console.WriteLine();
            Thread.Sleep(3000);
            return RedirectToAction("Tests", new { id = 2 } );
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

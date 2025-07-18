using System.Diagnostics;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuestBookContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, GuestBookContext context)
        {
            _logger = logger;
            _context = context;
        }

        //3.1.2 在HomeController中加入讀取Book資料表的程式
        public IActionResult Index()
        {
            var result = _context.Book.Where(b => b.Photo != null).OrderByDescending(s => s.CreateDate).Take(5).ToList();

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


//兩個view放在一起有兩種做法：
//1. 做一個view model，將兩個view的資料放在一起
//2. 做一個view component，將兩個view的資料放在一起
//以上皆是使用後端的方式去做

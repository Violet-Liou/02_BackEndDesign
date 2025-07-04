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

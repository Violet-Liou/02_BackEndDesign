using Microsoft.AspNetCore.Mvc;
using NorthWindStore.Models;
using NorthWindStore.ViewModel;


namespace NorthWindStore.Controllers
{
    public class VMProductController : Controller
    {
        private readonly NorthwindContext _context;

        public VMProductController(NorthwindContext context) //建構子，接收NorthwindContext物件
        {
            _context = context; //將傳入的context賦值給_context
        }

        //5.8.4 撰寫VMProductController裡新的IndexViewModel Action
        public IActionResult IndexVModel(int id = 1)
        {
            //string id =>從IndexViewModel.cshtml的超連結傳入的參數，代表系所編號

            VMProduct product = new VMProduct() //建立VMProduct物件
            {
                Categories = _context.Categories.ToList(), //抓Categories所有資料
                Products = _context.Products.Where(s => s.CategoryID == id).ToList() //依據Categories去讀取出相應的Products資料
            };

            ViewData["CateName"] = _context.Categories.Find(id).CategoryName;
            ViewData["CateID"] = id; //將系所編號存入ViewData，以便在視圖中使用

            return View(product);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

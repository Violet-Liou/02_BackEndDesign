using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.ViewComponents
{
    public class VCBookTopThree: ViewComponent //繼承自ViewComponent類別 (與Controller寫法相同)
    {
        //1. 宣告物件
        private readonly GuestBookContext _context;

        //2. 使用建構子注入
        public VCBookTopThree(GuestBookContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
           var books = _context.Book.OrderByDescending(b => b.CreateDate).Take(4); //取前三筆資料

            return View(books); //回傳結果到View
        }
    }
}

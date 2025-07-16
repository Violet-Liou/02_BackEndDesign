using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;

namespace CodeFirst.ViewComponents
{
    //2.3.3 VCReBooks class繼承ViewComponent(注意using Microsoft.AspNetCore.Mvc;)  
    public class VCReBooks : ViewComponent //繼承
    {
        public readonly GuestBookContext _context;

        public VCReBooks(GuestBookContext context)
        {
            _context = context;
        }

        //4.3.4 在VCRebooks ViewComponent中加入isDel參數判斷是否呈現Delete View
        //2.3.4 撰寫InvokeAsync()方法取得回覆留言資料
        public async Task<IViewComponentResult> InvokeAsync(string bookId, bool isDel = false)
        {
            //select * from ReBooks where BookID = bookId
            var rebook = await _context.ReBook.Where(r => r.BookID == bookId)
                                              .OrderByDescending(r => r.CreateDate) // 按照CreateDate降序排列
                                              .ToListAsync(); // 取得回覆留言資料

            if (isDel)
                return View("Delete", rebook);

            return View(rebook);
        }
    }
}

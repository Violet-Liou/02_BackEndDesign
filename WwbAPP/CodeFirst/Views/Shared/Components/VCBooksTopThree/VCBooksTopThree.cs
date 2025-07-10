using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Views.Shared.Components.VCBooksTopThree
{
    public class VCBooksTopThree
    {
        public PostBooksController(GuestBookContext context)
        {
            _context = context;
        }

        public IActionConstraint Invoke(ViewContext context)
        {
            // 這裡可以實現你的邏輯來決定是否要執行這個 View Component
            // 例如，你可以檢查某些條件，然後返回 true 或 false
            var result = await _context.Book.OrderByDescending(s => s.CreateDate).Take(3).ToListAsync();

            return true; // 簡單示範，總是返回 true


        }
    }
}

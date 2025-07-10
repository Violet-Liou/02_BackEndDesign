using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;
using System.Drawing;

//使用ajax做前後端分離的留言板-回覆留言

namespace CodeFirst.Controllers
{
    public class ReBooksController : Controller
    {
        private readonly GuestBookContext _context;

        public ReBooksController(GuestBookContext context)
        {
            _context = context;
        }


        // GET: ReBooks/Create
        public IActionResult Create(string BookID)
        {
            //ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID");
            ViewData["BookID"] = BookID;  //2.5.8 傳遞BookID參數
            return View();
        }

        // POST: ReBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReBookID,Description,Author,CreateDate,BookID")] ReBook reBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reBook);
                await _context.SaveChangesAsync();

                //2.5.9 新增留言後，導向到PostBooks/Display
                //return RedirectToAction(nameof(Index));}
                //return RedirectToAction("Display", "PostBooks", new { id = reBook.BookID });
                //問題：頁面重新載入後，會回到留言板的首頁，但是會回到頁面的最上方 >> UI很糟糕，需要讓使用者自己滾動到回覆區

                return Json(reBook);
            }
            //ViewData["BookID"] = new SelectList(_context.Book, "BookID", "BookID", reBook.BookID);
            //return View(reBook);
            return Json(reBook);
            //return View 會重新requset，但return Json(reBook)不會重新requset，這樣就不會回到頁面的最上方
        }

        public IActionResult GetReBookByViewComponent(string BookID)
        {
            return ViewComponent("VCReBooks", new { bookId = BookID });
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;

namespace CodeFirst.Controllers
{
    public class BooksManageController : Controller
    {
        private readonly GuestBookContext _context;

        public BooksManageController(GuestBookContext context)
        {
            _context = context;
        }

        // GET: BooksManage
        public async Task<IActionResult> Index()
        {
            return View(await _context.Book.ToListAsync());
        }

        //// GET: BooksManage/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var book = await _context.Book
        //        .FirstOrDefaultAsync(m => m.BookID == id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(book);
        //}

        //// GET: BooksManage/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: BooksManage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("BookID,Title,Description,Author,Photo,CreateDate")] Book book)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(book);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(book);
        //}

        // GET: BooksManage/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var book = await _context.Book.FindAsync(id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(book);
        //}

        // POST: BooksManage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("BookID,Title,Description,Author,Photo,CreateDate")] Book book)
        //{
        //    if (id != book.BookID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(book);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BookExists(book.BookID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(book);
        //}


        //★GET的方式，會可以讓使用者從瀏覽器的網址列直接輸入網址來刪除資料，這樣不安全，所以改成POST的方式來刪除資料
        //// GET: BooksManage/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var book = await _context.Book
        //        .FirstOrDefaultAsync(m => m.BookID == id);
        //    if (book == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(book);
        //}

        // POST: BooksManage/Delete/5
        //[HttpPost, ActionName("Delete")] >>同名的Delete action已刪除，所以也不需要在指定action name
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _context.Book.FindAsync(id);

            if(book == null)
            {
                return View();
            }

            //4.4.7 在BooksManageController中的Delete Action加入刪除圖片的程式
            //組合路徑
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BookPhotos", book.Photo??string.Empty);
            // book.Photo??string.Empty >>如果book.Photo為null，則使用空字串，避免路徑組合時出現null值

            if (System.IO.File.Exists(filePath))
                //刪除圖片檔案
                System.IO.File.Exists(filePath);


                if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();//整頁處理
            return RedirectToAction(nameof(Index));
        }

        //4.4.1 在BooksManageController加人中的DeleteReBook Action
        [HttpPost]
        public async Task<IActionResult> DeleteReBook(string id)
        {
            var reBook = await _context.ReBook.FindAsync(id);

            if (string.IsNullOrEmpty(id))
            {
                return Json(reBook);
            }
            
            _context.ReBook.Remove(reBook);
            await _context.SaveChangesAsync();
            return Json(reBook);
        }

        //4.4.4 在BooksManageController中加入GetRebookByViewComponent Action
        public IActionResult GetRebookByViewComponent(string id)
        {
            //使用ViewComponent取得回覆留言資料
            return ViewComponent("VCReBooks", new { bookId = id, isDel = true });
        }
    }
}

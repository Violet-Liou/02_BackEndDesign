using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DBFirst.Models;

namespace DBFirst.Controllers
{
    public class tStudentsController : Controller
    {
        //2.2.2 將原先既有的程式碼(如下)註解掉
        private readonly dbStudentsContext _context;

        public tStudentsController(dbStudentsContext context)
        {
            _context = context;
        }

        //2.2.1 撰寫建立DbContext物件的程式
        //dbStudentsContext _context = new dbStudentsContext(); // 直接建立 dbStudentsContext 的實例

        // GET: tStudents
        public async Task<IActionResult> Index()
        {
            return View(await _context.tStudent.ToListAsync());
        }

        // GET: tStudents/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tStudent = await _context.tStudent
                .FirstOrDefaultAsync(m => m.fStuId == id);
            if (tStudent == null)
            {
                return NotFound();
            }

            return View(tStudent);
        }

        // GET: tStudents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: tStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("fStuId,fName,fEmail,fScore")] tStudent tStudent)
        {
            //SELECT * FROM tStudent WHERE fStuId = '127377'

            //Linq
            var result = await _context.tStudent.FindAsync(tStudent.fStuId);

            if (result != null)//表示已經有此學號存在資料庫
            {
                ViewData["ErrorMessage"] = "學號已存在，請重新輸入。"; //將錯誤訊息傳遞到View
                //另一個呈現錯誤訊息的方法
                //ModelState.AddModelError("fStuId", "學號已存在，請重新輸入。"); //將錯誤訊息加入到模型狀態中
                return View(tStudent);
            }

            if (ModelState.IsValid) //模型驗證是否完全符合規則
            {
                _context.Add(tStudent); //將資料加入到資料庫的tStudent表中
                await _context.SaveChangesAsync(); //正式寫入資料庫 SaveChangesAsync 代表用非同步去處理資料
                return RedirectToAction(nameof(Index));  //回到Index 這個action

                //如果function是async，裡面一定會有await 
            }
            return View(tStudent);
        }

        // GET: tStudents/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tStudent = await _context.tStudent.FindAsync(id);
            if (tStudent == null)
            {
                return NotFound();
            }
            return View(tStudent);
        }

        // POST: tStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("fStuId,fName,fEmail,fScore")] tStudent tStudent)
        {
            if (id != tStudent.fStuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tStudent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tStudentExists(tStudent.fStuId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tStudent);
        }

        // GET: tStudents/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tStudent = await _context.tStudent
                .FirstOrDefaultAsync(m => m.fStuId == id);
            if (tStudent == null)
            {
                return NotFound();
            }

            return View(tStudent);
        }

        // POST: tStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tStudent = await _context.tStudent.FindAsync(id);
            if (tStudent != null)
            {
                _context.tStudent.Remove(tStudent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tStudentExists(string id)
        {
            return _context.tStudent.Any(e => e.fStuId == id);
        }
    }
}

using DBFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DBFirst.Controllers
{
    public class MyStudentsController : Controller
    {
        //4.1.4 撰寫建立DbContext物件的程式
        dbStudentsContext db = new dbStudentsContext(); // 直接建立 dbStudentsContext 的實例

        
        //讀取tStudent資料表的資料
        public IActionResult Index()
        {
            //4.2.1 撰寫Index Action程式碼

            //SELECT * FROM tStudent

            //Linq
            //第一種寫法
            //var result = from s in db.tStudent
            //             select s;
            //第二種寫法(較短)
            //var result = db.tStudent.ToList(); //ToList =>將查詢結果轉換為List<tStudent>型別

            var result = db.tStudent.Include(t => t.Department).ToList(); //關聯Department資料表

            //★重要
            return View(result); //要記得回傳result，否則無法正確顯示表單!!!!!!
        }

        //4.3   建立同步執行的Create Action
        //4.3.1 撰寫Create Action程式碼(需有兩個Create Action)
        //4.3.2 建立Create View
        public IActionResult Create()
        {
            //5.5.3 修改 Create Action
            ViewData["Dept"] = new SelectList(db.Department, "DeptID", "DeptName"); //建立給下拉選單的資料來源
            //new SelectList =>做成一個select下拉選單
            //_context.Department =>要用哪一個資料表
            //DeptID是實際上要讀取到的值
            //DeptName是要顯示的值
            return View();
        }

        //4.3.7 加入Token驗證標籤 =>Token 驗證標籤是用來防止CSRF攻擊的，確保表單提交的安全性。
        //每次權杖都會變更，這樣就可以防止CSRF攻擊。
        //圖形驗證碼也是在防這個
        [HttpPost]
        [ValidateAntiForgeryToken] //如沒有這行，就算表單有Token也沒有用
        public IActionResult Create(tStudent student) //用複雜係結 =>因為有模型
        {
            //檢查學號是否重複
            var result = db.tStudent.Find(student.fStuId); //使用Find方法查詢資料庫中是否已存在相同的學號
            if(result != null)
            {
                ViewData["ErrorMessage"] = "學號已存在，請重新輸入。"; //將錯誤訊息傳遞到View
                return View(student); //如果學號已存在，則返回Create視圖並顯示錯誤訊息
            }

            //把表單送出來的資料存入資料庫

            if (ModelState.IsValid)
            {
                //1. 在模型新增一筆資料 
                db.tStudent.Add(student); //假設有一個db物件，代表資料庫上下文

                //2. 回寫資料庫
                db.SaveChanges(); //轉譯SQL，執行INSERT INTO tSeudent(fStuId, fName, fEmail, fScore)將變更儲存到資料庫

                return RedirectToAction("Index"); //新增成功後，導向到Index Action
            }

            //如果模型驗證失敗，則回到Create View
            return View(student);
        }

        //4.4.1 撰寫Edit Action程式碼(需有兩個Edit Action)
        //此部分呈現的是編輯學生資料的功能，包含讀取學生資料和更新學生資料。
        public ActionResult Edit(string id)
        {
            ViewData["TimeNow"] = DateTime.Now; //抓取時間

            var result = db.tStudent.Find(id); //使用Find方法查尋學號是否存在

            if (result == null)
            {
                return NotFound(); //如果找不到對應的學生資料，則返回404 Not Found
            }

            //5.5.5 修改 Edit Action
            ViewData["Dept"] = new SelectList(db.Department, "DeptID", "DeptName"); //建立給下拉選單的資料來源

            return View(result);
        }

        //此部分呈現的是 點擊提交表單後，會需要做的相關判斷
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(string id, tStudent student)
        {
            if (id != student.fStuId) //檢查傳入的學號是否與表單資料的學號一致
            {
                return View(student); //如果不一致，則回到Edit View
            }

            if (ModelState.IsValid)
            {
                db.tStudent.Update(student);
                db.SaveChanges();
                return RedirectToAction("Index"); //編輯完成後，導向到Index Action

            }
            return View(student);

        }

        //4.5.1 撰寫Delete Action程式碼
        public IActionResult Delete(string id)
        {
            //DELETE FROM tStudent WHERE fStuId = id
            var result = db.tStudent.Find(id); //使用Find方法查詢學號是否存在

            if(result == null)
            {
                return NotFound(); //如果找不到對應的學生資料，則返回404 Not Found
            }

            db.tStudent.Remove(result); //如果找到對應的學生資料，則將其從模型資料中刪除
            db.SaveChanges(); //將變更儲存到資料庫

            return RedirectToAction("Index"); //刪除完成後，導向到Index Action
        }

    }
}

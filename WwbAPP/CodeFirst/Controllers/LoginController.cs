using System.Security.Claims;
using CodeFirst.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers
{
    public class LoginController : Controller
    {
        private readonly GuestBookContext _context;

        public LoginController(GuestBookContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)// 修改方法為非同步，並將回傳型別改為 Task<IActionResult>
        {
            var user = _context.Login
                .FirstOrDefault(u => u.Account == login.Account && u.Password == login.Password);
            if (user != null)//if登入正確
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Account),
                    new Claim(ClaimTypes.Role, "Manager") // 假設所有登入的用戶都是管理者
                };

                // 使用 ClaimsIdentity 建立 ClaimsIdentity
                // 就像在辦護照，在系統中有一個身分識別
                var claimsIdentity = new ClaimsIdentity(claims, "ManagerLogin");
                // 使用 ClaimsIdentity 建立 ClaimsPrincipal
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                //await HttpContext.SignInAsync("ManagerLogin", claimsPrincipal);//修正為非同步方法

                return RedirectToAction("Index", "BooksManage");//登入成功，跳轉到BooksManage的Index頁面
            }

            ViewData["Error"] = "帳號或密碼錯誤，請重新輸入。";

            return View(login);//登入錯誤，返回登入頁面
        }
    }
}

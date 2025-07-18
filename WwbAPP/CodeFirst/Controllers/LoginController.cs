using System.Security.Claims;
using CodeFirst.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers
{
    //[Authorize]// 這個特性用來限制只有已登入的用戶才能訪問此Controller
    //>> 因已在Program.cs中設定全域驗證，所以不需要在這裡再加上[Authorize]特性了 【白名單】
    //[AllowAnonymous] // 這個特性用來允許未登入的用戶訪問此Controller
    public class LoginController : Controller
    {
        private readonly GuestBookContext _context;

        public LoginController(GuestBookContext context)
        {
            _context = context;
        }

        //[Authorize]// 這個特性用來限制只有已登入的用戶才能訪問此action
        [AllowAnonymous]
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

                await HttpContext.SignInAsync("ManagerLogin", claimsPrincipal);
                // 使用 SignInAsync 方法登入，這個方法會將使用者的身分識別存儲在 Cookie 中

                return RedirectToAction("Index", "BooksManage");//登入成功，跳轉到BooksManage的Index頁面
            }

            ViewData["Error"] = "帳號或密碼錯誤，請重新輸入。";

            return View(login);//登入錯誤，返回登入頁面
        }


        //5.2.9 建立 Logout 方法
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("ManagerLogin");
            // 使用 SignOutAsync 方法登出
            //清除登入狀態（清除 Cookie的ManagerLogin紀錄）
            // Cookie是放在client端(瀏覽器上)的，所以在這邊清除 Cookie並不會導致其他使用者的 Cookie被清除

            return RedirectToAction("Login", "Login");// 登出後跳轉到 Home 的 Index 頁面
        }
    }
}

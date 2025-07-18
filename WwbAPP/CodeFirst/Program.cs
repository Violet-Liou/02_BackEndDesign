using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(//options=>
    //options.Filters.Add(new AuthorizeFilter())
// 在Program.cs內加入防止CSRF攻擊的設定
//用 AuthorizeFilter 來限制所有的Controller和Action都需要授權才能訪問 >>全域驗證
//(這樣就不需要在每個Controller上加上[Authorize]特性了)
);

//1.2.4 在Program.cs內以依賴注入的寫法註冊讀取連線字串的服務
// 註冊成Services使用
builder.Services.AddDbContext<GuestBookContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GuestBookConnection")));

builder.Services.AddAuthentication("ManagerLogin")
    .AddCookie("ManagerLogin", options =>
    {
        options.LoginPath = "/Login/Login"; // 設定登入頁面路徑
        options.LogoutPath = "/Login/Logout"; // 設定登出頁面路徑
        options.AccessDeniedPath = "/Home/Error"; // 設定存取拒絕頁面路徑
    });

///////////////////////////////////////////////////////////////////////////////////////////////////////
var app = builder.Build();

//1.3.4 在Program.cs撰寫啟用Initializer的程式
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    SeedData.Initialize(service);
}

//6.1.3 修改 Program.cs中的程式碼，將是否在開發模式下的錯誤處理判斷註解掉
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) //如果不是在關發環境裡，就不使用Error頁面, if拿掉，就會全部都使用
{
    app.UseExceptionHandler("/Home/Error");

    //6.2.1 在Program.cs中的程式碼註冊處理HttpNotFound(404)錯誤的Error Handler
    app.UseStatusCodePagesWithReExecute("/Home/Error"); //這行會將所有的錯誤頁面都導向到HomeController的Error方法
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

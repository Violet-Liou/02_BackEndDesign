using DBFirst.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//註冊成Services使用
builder.Services.AddDbContext<dbStudentsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbStudentsConnection")));

//【上方是在寫設定、註冊服務】
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///【下方是要啟動上面寫的設定】 所以設定必須寫在啟動之前
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

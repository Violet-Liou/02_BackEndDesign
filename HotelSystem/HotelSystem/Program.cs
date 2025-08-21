using HotelSystem.Access.Data;
using HotelSystem.Filters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => {
    options.Filters.Add<LogFilter>();
});


builder.Services.AddDbContext<HotelSysDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelSysDBConnection")));


builder.Services.AddDbContext<HotelSysDBContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelSysDBConnection")));







var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();





// 1. �����U�S�����
//�h�h������
app.MapControllerRoute(
    name: "EmployeeRole",
    pattern: "Manager/{action=Index}/{id?}",
    defaults: new {area="Admin", controller = "EmployeeRoles" });



//�ۭq�R�A���|
app.MapControllerRoute(
    name: "about",
    pattern: "about-us",
    defaults: new {  controller = "Home", action = "Privacy" });



// �a���h�ӰѼƪ�����
app.MapControllerRoute(
    name: "order",
    pattern: "order/{orderId}/{productId}",
    defaults: new { controller = "Order", action = "Detail" });




//2. �A���U�ϰ����
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
        name: "admin",
        pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");

// 3. �̫���U�w�]����

app.MapControllerRoute(
    name: "aaa",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



//app.MapControllerRoute(
//    name: "default",
//    pattern: "{area}/{controller}/{action}/{id?}");





app.Run();

using HotelSystem.Access.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//6.1.4 在Program.cs加入使用appsettings.json中的連線字串程式碼(這段必須寫在var builder這行後面)
builder.Services.AddDbContext<HotelSysDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelSysDBConnection")));

builder.Services.AddDbContext<HotelSysDBContext2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HotelSysDBConnection")));

////////////////////////////////////////////////////////////////////////////////
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

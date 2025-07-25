using HotelSystem.Access.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

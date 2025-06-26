using Microsoft.EntityFrameworkCore;
using NorthWindStore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//µù¥U¦¨Services¨Ï¥Î
builder.Services.AddDbContext<NorthwindContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbNorthWindConnection")));


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

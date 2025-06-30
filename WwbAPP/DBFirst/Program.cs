using DBFirst.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//���U��Services�ϥ�
builder.Services.AddDbContext<dbStudentsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbStudentsConnection")));

//�i�W��O�b�g�]�w�B���U�A�ȡj
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///�i�U��O�n�ҰʤW���g���]�w�j �ҥH�]�w�����g�b�Ұʤ��e
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

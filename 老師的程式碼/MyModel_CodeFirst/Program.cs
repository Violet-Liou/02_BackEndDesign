using System.Net;
using Microsoft.AspNetCore.Mvc.Authorization;

using Microsoft.EntityFrameworkCore;
using MyModel_CodeFirst.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(//options =>
                                         //options.Filters.Add(new AuthorizeFilter())  //��Filter���覡�M�Υ�������
);

//1.2.4 �bProgram.cs���H�̿�`�J���g�k���UŪ���s�u�r�ꪺ�A��(food panda�BUber Eats)
//���`�N�{������m�����n�bvar builder = WebApplication.CreateBuilder(args);�o�y����B�bvar app = builder.Build();���e
builder.Services.AddDbContext<GuestBookContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GuestBookConnection")));

//5.2.6 �b Program.cs ���U Cookie Authentication
builder.Services.AddAuthentication("ManagerLogin").AddCookie("ManagerLogin", options =>
    {
        options.LoginPath = "/Login/Login"; // �]�w�n�J�������|(�Y�ݵn�J�ӥ��n�J�h�j��ɨ즹���|)
        options.LogoutPath = "/Login/Logout"; // �]�w�n�X�������|
        options.AccessDeniedPath = "/Home/Index"; // �]�w�s���ڵ��������|(�Y�w�n�J�������v������,�h�j��ɨ즹���|)
    });




/////////////////////////////////////////////////////////////////////
var app = builder.Build();

//1.3.4 �bProgram.cs���g�ҥ�Initializer���{��
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    SeedData.Initialize(service);
}

//6.1.3 �ק� Program.cs�����{���X�A�N�O�_�b�}�o�Ҧ��U�����~�B�z�P�_���ѱ�
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    //6.2.1 �bProgram.cs�����{���X���U�B�zHttpNotFound(404)���~��Error Handler
    app.UseStatusCodePagesWithReExecute("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

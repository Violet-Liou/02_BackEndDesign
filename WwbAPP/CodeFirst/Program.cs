using Microsoft.EntityFrameworkCore;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(//options=>
    //options.Filters.Add(new AuthorizeFilter())
// �bProgram.cs���[�J����CSRF�������]�w
//�� AuthorizeFilter �ӭ���Ҧ���Controller�MAction���ݭn���v�~��X�� >>��������
//(�o�˴N���ݭn�b�C��Controller�W�[�W[Authorize]�S�ʤF)
);

//1.2.4 �bProgram.cs���H�̿�`�J���g�k���UŪ���s�u�r�ꪺ�A��
// ���U��Services�ϥ�
builder.Services.AddDbContext<GuestBookContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GuestBookConnection")));

builder.Services.AddAuthentication("ManagerLogin")
    .AddCookie("ManagerLogin", options =>
    {
        options.LoginPath = "/Login/Login"; // �]�w�n�J�������|
        options.LogoutPath = "/Login/Logout"; // �]�w�n�X�������|
        options.AccessDeniedPath = "/Home/Error"; // �]�w�s���ڵ��������|
    });

///////////////////////////////////////////////////////////////////////////////////////////////////////
var app = builder.Build();

//1.3.4 �bProgram.cs���g�ҥ�Initializer���{��
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;

    SeedData.Initialize(service);
}

//6.1.3 �ק� Program.cs�����{���X�A�N�O�_�b�}�o�Ҧ��U�����~�B�z�P�_���ѱ�
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) //�p�G���O�b���o���Ҹ̡A�N���ϥ�Error����, if�����A�N�|�������ϥ�
{
    app.UseExceptionHandler("/Home/Error");

    //6.2.1 �bProgram.cs�����{���X���U�B�zHttpNotFound(404)���~��Error Handler
    app.UseStatusCodePagesWithReExecute("/Home/Error"); //�o��|�N�Ҧ������~�������ɦV��HomeController��Error��k
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

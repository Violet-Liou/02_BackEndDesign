using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;
using MyWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.



//跨域存取政策
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


//builder.Services.AddControllers();

//4.7.10 在Program裡全域啟用 ReferenceHandler.Preserves
//在Program.cs中加入Json序列化的設定，讓前端可以正確讀取資料
//全域性的註冊，讓所有的Controller都使用這個設定，設定不會無窮參照
//builder.Services.AddControllers()
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); //指向到swagger的頁面
builder.Services.AddSwaggerGen();

//1.2.4 在Program.cs內以依賴注入的寫法註冊讀取連線字串的服務
// 註冊成Services使用
builder.Services.AddDbContext<GoodStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoodStoreConnection")));

//4.7.7 在Program裡註冊GoodStoreContext2的Service(※注意※原本註冊的GoodStoreContext不可刪掉)
builder.Services.AddDbContext<GoodStoreContextG2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoodStoreConnection")));

//8.1.6 在Program.cs裡註冊SomeService服務
builder.Services.AddScoped<SomeService>();

//8.2.3 在Program.cs裡註冊CategoryService
builder.Services.AddScoped<CategoryService>();

//8.3.3 在Program.cs裡註冊ProductService
builder.Services.AddScoped<ProductService>();

//9.2.2 在Program.cs內註冊HttpClient物件
builder.Services.AddScoped<HttpClient>();

//9.2.5 在Program.cs內註冊ThirdPartyService物件
builder.Services.AddScoped<ThirdPartyService>();

////////////////////////////////////////////////////////////////////////////////////////////
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyCorsPolicy");

//2.1.3 在Program.cs中加入app.UseStaticFiles(); (因為我們開的是 純WebAPI專案)
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action}/{id?}"
    );

app.UseAuthorization();

app.MapControllers();

app.Run();

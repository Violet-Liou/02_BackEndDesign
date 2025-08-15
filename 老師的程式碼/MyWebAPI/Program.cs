using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;
using MyWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//���s���F��
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});



//4.7.10 �bProgram�̥���ҥ� ReferenceHandler.Preserve
//builder.Services.AddControllers();
//9.3.6 �bProgram.cs�̱NAddControllers()�A�ȧאּAddControllersWithViews�A�Ȩñҥθ��Ѿ���Route()
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GoodStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoodStoreConnection")));

//4.7.7 �bProgram�̵��UGoodStoreContext2��Service(���`�N���쥻���U��GoodStoreContext���i�R��)
builder.Services.AddDbContext<GoodStoreContextG2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoodStoreConnection")));


//8.1.6 �bProgram.cs�̵��USomeService�A��
builder.Services.AddScoped<SomeService>();

//8.2.3 �bProgram.cs�̵��UCategoryService
builder.Services.AddScoped<CategoryService>();

//8.3.3 �bProgram.cs�̵��UProductService
builder.Services.AddScoped<ProductService>();

//9.2.2 �bProgram.cs�����UHttpClient����
builder.Services.AddScoped<HttpClient>();


//9.2.5 �bProgram.cs�����UThirdPartyService����
builder.Services.AddScoped<ThirdPartyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyCorsPolicy");


//2.1.3 �bProgram.cs���[�Japp.UseStaticFiles(); (�]���ڭ̶}���O ��WebAPI�M��)
app.UseStaticFiles();

app.UseRouting();
app.MapControllerRoute(
    name:"default",
    pattern:"{controller}/{action}/{id?}"
    );


app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using MyWebAPI.Models;

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


//builder.Services.AddControllers();

//4.7.10 �bProgram�̥���ҥ� ReferenceHandler.Preserves
//�bProgram.cs���[�JJson�ǦC�ƪ��]�w�A���e�ݥi�H���TŪ�����
//����ʪ����U�A���Ҧ���Controller���ϥγo�ӳ]�w�A�]�w���|�L�a�ѷ�
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//1.2.4 �bProgram.cs���H�̿�`�J���g�k���UŪ���s�u�r�ꪺ�A��
// ���U��Services�ϥ�
builder.Services.AddDbContext<GoodStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoodStoreConnection")));

//4.7.7 �bProgram�̵��UGoodStoreContext2��Service(���`�N���쥻���U��GoodStoreContext���i�R��)
builder.Services.AddDbContext<GoodStoreContextG2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoodStoreConnection")));

////////////////////////////////////////////////////////////////////////////////////////////
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

app.UseAuthorization();

app.MapControllers();

app.Run();

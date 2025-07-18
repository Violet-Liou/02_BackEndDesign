using Microsoft.AspNetCore.Mvc;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}




//1     �s�@�ڪ��Ĥ@��Web API(Restful API)

//1.1   �إ�CRUD API Ccontroller 
//1.1.1 �bControllers��Ƨ��W���k����[�J�����
//1.1.2 ��������I��uAPI�v�� �����D����ܡu����Ū��/�g�J�ʧ@��API����v�����U�u�[�J�v�s
//1.1.3 �ɦW�ϥιw�]��ValuesController.cs �Y�i�A���U�u�s�W�v�s
//      ���ڭ̷|�o��@�Ӥw�g���g�n��CRUD�[�c��API Ccontroller��
//      ����Ccontroller�����g����ŦXRest�A�ϥ�Get�BGet/{id}�BPost�BPut�BDelete �i��U���ʧ@��


//1.2   �w��Swagger Tool(�p�G���ݭn����)
//1.2.1 �ϥ�NuGet(�M�צW�٤W���k����޲zNuGet�M��)�w��Swashbuckle.AspNetCore�M��
//1.2.2 �bProgram.cs�����U�αҥ�Swagger
//1.2.3 �w�˧���а��楻�M�������A������
//1.2.4 �b���}�C��Jhttp://localhost:xxxx/swagger/index.html (�䤤xxxx�O�z��port)
//1.2.5 ���դ��A��Swagger
//      ��Swagger�O�Ѥ@�a�sSmartBear�����q�ҵo��A�ݩ�L�v�ϥΪ�OpenAPI�M��A�H�ϥΩ�API�}�o�ɪ����ա�
//      ���H���}�o�h�ϥ�Postman�o�ӳn��i��API���աA�ثeSwagger�����D�y��
//      ���Y�إ߱M�׮ɬ�WebAPI�M�רäĿ�Open API�ASwagger�N�����w�˦b�M�פW��  


//1.3   �ϥ�Swagger Tool�Ӷi��ValuesController API���ާ@����
//1.3.1 �ק�Get Action�����e�ô���
//1.3.2 �i��W�[Action�B�ק虜���f������
//1.3.3 �ק�ValuesController�����Ѥ�����}�ô��� ([Route("api/[controller]")])
//      ��Web API�]���S��UI�A�Q���s�����u��NGet���ʧ@�i����աA�L�k��Post�BPut��Delete���ʧ@�i����ա� 
//      ���٦��@�ӱj�j��API�n��sPostman�A�H�e�٨S��Swagger�ɤj���O��Postman�i����ա�
//      ���]���o�̪��ާ@�ت��O���xSwagger���Ϊk�A�H�QWeb API�b�}�o�ɯ�ϥΥ��Ӷi����ա�



/////////////////////////////////////////////////////////////////////////////
///

//2     �d�ұM�׶}�o�ǳ�

//2.1   �Q�ί����اQ�d�ұM������
//2.1.1 �إ�GoodStore�d�Ҹ�Ʈw
//2.1.2 �NProductPhotos��Ƨ��Τ��t���ɮש�ܱM�פ���wwwroot�U
//2.1.3 �bProgram.cs���[�Japp.UseStaticFiles(); (�]���ڭ̶}���O ��WebAPI�M��)
//2.1.4 �b�s��������J�uhttp://localhost:XXXX/ProductPhotos/A3001.jpg�v(XXXX���z��port)���լO�_��ݨ�Ӥ�


//2.2   �إ߱M�׻P��Ʈw���s�u
//2.2.1 �ϥ�DB First�إ� Model
//2.2.2 �ϥ�NuGet(�M�צW�٤W���k����޲zNuGet�M��)�w�ˤU�C�M��
//      (1) Microsoft.EntityFrameworkCore.SqlServer
//      (2) Microsoft.EntityFrameworkCore.Tools 
//2.2.3 ��M��޲z���D���x(�˵� > ��L���� > �M��޲z���D���x)�U���O
//      Scaffold-DbContext "Data Source=���A����};Database=GoodStore;TrustServerCertificate=True;User ID=�b��;Password=�K�X" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -NoOnConfiguring  -UseDatabaseNames -NoPluralize -Force
//      �Y���\���ܡA�|�ݨ�Build succeeded.�r���A�æbModels��Ƨ��̬ݨ�GoodStoreContext.cs�BCategory.cs�BProduct.cs����Ʈw�������O��
//2.2.4 �bappsettings.json�ɤ����g�s�u�r��(ConnectionString)
//2.2.5 �bProgram.cs���UDbContext����(GoodStoreContext.cs)�ë��wappsettings.json�����s�u�r��{���X(�o�q�����g�bvar builder�o��᭱)
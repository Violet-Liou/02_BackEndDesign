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


/////////////////////////////////////////////////////////////////////////////


//3     �s�@��CRUD �� Restful API(Web API)

//3.1   �إ�Category��ƪ� API Ccontroller 
//3.1.1 �bControllers��Ƨ��W���k����[�J�����
//3.1.2 ��������I��uAPI�v�� �����D����ܡu�ϥ�Entity Framework����ʧ@��API����v�����U�u�[�J�v�s
//3.1.3 �b��ܤ�����]�w�p�U
//      �ҫ����O: Category(MyStoreAPI.Models)
//      ��Ƥ��e���O: GoodStoreContext(MyStoreAPI.Models)
//      ����W�٨ϥιw�]�Y�i(CategoriesController)
//      ���U�u�s�W�v�s
//3.1.4 �ק�API�������Ѭ��uapi[controller]�v


//3.2   �إ�Product��ƪ� API Ccontroller 
//3.2.1 �bControllers��Ƨ��W���k����[�J�����
//3.2.2 ��������I��uAPI�v�� �����D����ܡu�ϥ�Entity Framework����ʧ@��API����v�����U�u�[�J�v�s
//3.2.3 �b��ܤ�����]�w�p�U
//      �ҫ����O: Product(MyStoreAPI.Models)
//      ��Ƥ��e���O: GoodStoreContext(MyStoreAPI.Models)
//      ����W�٨ϥιw�]�Y�i(ProductsController)
//      ���U�u�s�W�v�s
//3.2.4 �ק�API�������Ѭ��uapi[controller]�v
//���ϥ�Swagger Tool���O��W����� API�i��ާ@���ա�



//3.3   ��Ʊ����ӷ�
//3.3.1 �HSwagger���լd�ݥثeCategoriesController��ProductsController����ƨӷ�
//3.3.2 �NCategoriesController��ProductsController����Action�ѼƧ��ܸ�Ʊ����ӷ��åHSwagger���դΥ�e���
//����Ʊ����ӷ����[���ܭ��n�A�����t�X��@�����աA�z�LSwagger�[�d�ܤơA�N�ા�D�ϥήɾ���

//���ɥR������
//[FromBody]�GHTTP �ШD���D�餺�e(Body)�j�w��Action���ѼƤW�A�q�`�Ω���oJSON�BXML�Ψ�L�榡����r��ơC
//[FromForm]�G�N�Ӧ�HTTP�ШD�D�骺����Ƹj�w��Action���ѼƤW�A�`�Ω󱵦��Ӧ۪��form���檺��ơA�Ҧp application/x-www-form-urlencoded �� multipart/form-data�C
//[FromQuery]�G�NURL�����ѼƸj�w��Action���ѼƤW�A��A�Ʊ�q URL �����o��ƮɨϥΡC
//[FromHeader]�G�NHTTP�ШD�������Y�ȸj�w��Action���ѼƤW�A�A�X�qRequest Header�����o��ơA�ҦpAuthentication Token�BClient�ݸ�T���C
//[FromRoute]�G�NURL���Ѥ����ѼƸj�w��Action���ѼƤW�A��URL���Y�����O�ʺA���A�ݭn���o�o�Ǹ��ѰѼƮɨϥΡC


/////////////////////////////////////////////////////////////////////////////


/////////////////////////////////////////////////////////////////////////////


//4     �ϥ�Get���o���

//4.1   ���o��ƲM��(ProductsController�̪��Ĥ@��Get Action)
//4.1.1 ���ϥ�Swagger���դ��[�d�ثeProduct����ƨ��o���p
//4.1.2 �ϥ�Include()�P�ɨ��o���p���
//4.1.3 �ϥ�Where()���ܬd�ߪ�����ô���
//4.1.4 �ϥ�OrderBy()�����ƧǤ�k���ܸ�ƱƧǨô���
//4.1.5 �ϥ�Select()����S�w�����ô���
//4.1.6 �ϥ�Swagger���դ��[�d�ثeProduct����ƨ��o���p�A�öi��W�z��������
//���o�̦��@�ǭ��n���[�����������γz�L��@�ӥ[�j�L�H�A�ר��ƪ��������p�S�ʱN�|�v�T��@���覡��
//���o�ʹ`���ѷӮɥi�ϥ�JsonIgnore�ӸѨM��


//4.2   �ϥ�DTO(Data Transfer Object)��ƶǿ骫��
//4.2.1 �إ�DTOs��Ƨ�(�o�ӨB�J�i���ݨD�өw)�ө�m�����ɮ�
//4.2.2 �إ�ProductDTO���O
//4.2.3 ��gProductsController�̪�Get Action
//4.2.4 �ϥ�Swagger����
//���Q�n����S�w���̨嫬����k�N�O�ϥ�DTO�Ӷǿ顰



//4.3   ���o�S�w���(ProductsController�̪��ĤG��Get Action)
//4.3.1 ���ϥ�Swagger���դ��[�d�ثeProduct����ƨ��o���p(�z�ѰѼƤΤ����f)
//4.3.2 �ϥ�Include()�P�ɨ��o���p��ƨèϥ�ProductDTO�Ӷǻ����
//4.3.3 �ϥ�Swagger����
//���o�ʹ`���ѷӮɥi�ϥ�JsonIgnore�ӸѨM��
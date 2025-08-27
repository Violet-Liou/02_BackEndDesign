using Microsoft.AspNetCore.Mvc;
using MyWebAPI.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AARController : ControllerBase
    {

        private readonly HttpClient _httpClient;

        public AARController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // GET: api/<AARController>
        [HttpGet]
        public async Task<ActionResult<AccommodationApiResponse>> Get()
        {
            string url = "https://openapi.kcg.gov.tw/Api/Service/Get/a182559b-5f14-439d-ad4e-512c0a7291bb";
            var response = await _httpClient.GetStringAsync(url);

            var result = JsonConvert.DeserializeObject<AccommodationApiResponse>(response);

            if (result == null)
                return StatusCode(500, "取得資料失敗");

            return Ok(result);
        }

        //// GET api/<AARController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<AARController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<AARController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<AARController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace MyWebAPI.Models
{
    public class AccommodationApiResponse
    {
        [JsonProperty("data")]
        public List<Accommodation> Data { get; set; }

        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }

    public class Accommodation
    {
        [JsonProperty("Seq")]
        public int Seq { get; set; }

        [JsonProperty("民宿登記證編號")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("中文名稱")]
        public string Name { get; set; }

        [JsonProperty("地址")]
        public string Address { get; set; }

        [JsonProperty("電話或手機")]
        public string Phone { get; set; }

        [JsonProperty("合計總房間數")]
        public string RoomCount { get; set; }
    }

    //[ApiController]
    //[Route("api/[controller]")]
    //public class AARController : ControllerBase
    //{
    //    private readonly HttpClient _httpClient;

    //    public AARController(HttpClient httpClient)
    //    {
    //        _httpClient = httpClient;
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<AccommodationApiResponse>> Get()
    //    {
    //        string url = "https://openapi.kcg.gov.tw/Api/Service/Get/a182559b-5f14-439d-ad4e-512c0a7291bb";
    //        var response = await _httpClient.GetStringAsync(url);

    //        var result = JsonConvert.DeserializeObject<AccommodationApiResponse>(response);

    //        if (result == null)
    //            return StatusCode(500, "取得資料失敗");

    //        return Ok(result);
    //    }
    //}
}
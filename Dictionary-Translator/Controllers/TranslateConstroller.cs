using Dictionary_Translator.Models;
using Dictionary_Translator.Secrets;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dictionary_Translator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslateConstroller : Controller
    {
        [HttpPost]
        [Route("translate")]
        public async Task<IActionResult> Translate([FromForm] string valueToTranslate)
        {
            if (String.IsNullOrEmpty(valueToTranslate))
            {
                return BadRequest();
            }

            string privateKey = SecretsManager.GetYandexAPIKey();
            string url = @$"https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={privateKey}&lang=en-ru&text={valueToTranslate}";

            using HttpClient httpClient = new HttpClient();
            using HttpRequestMessage requestToTranslate = new HttpRequestMessage(HttpMethod.Get, url);
            
            var responseFromServer = await httpClient.SendAsync(requestToTranslate);
            string translatedTextJSON = responseFromServer.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrEmpty(translatedTextJSON))
            {
                //TODO
            }

            Root translatedText =  JsonConvert.DeserializeObject<Root>(JObject.Parse(translatedTextJSON).ToString());

            if(translatedText is null)
            {
                //TODO
            }




            return Ok();
        }
    }
}

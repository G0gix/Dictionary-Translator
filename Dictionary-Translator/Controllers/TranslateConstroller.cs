using CoreLibrary.Logger;
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
        static ILogger Logger;

        public TranslateConstroller()
        {
            Logger = new LogToFile(Environment.CurrentDirectory + "\\Logging\\log.txt");
        }

        [HttpPost]
        [Route("translate")]
        public async Task<IActionResult> Translate([FromForm] string valueToTranslate)
        {
            Guid requestId = Guid.NewGuid();
            
            try
            {
                Logger.Log(LogLevel.Info, $"Request received. Id: {requestId}");

                if (String.IsNullOrEmpty(valueToTranslate))
                {
                    Logger.Log(LogLevel.Info, $"Request {requestId} finished. Input parametr is null or empty");
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
                    Logger.Log(LogLevel.Error, $"Request {requestId} finished. Response from Translate server Is Null Or Empty");
                    //TODO google sheets
                    return BadRequest();
                }

                Root translatedText = JsonConvert.DeserializeObject<Root>(JObject.Parse(translatedTextJSON).ToString());

                if (translatedText is null)
                {
                    //TODO Google Sheets
                    Logger.Log(LogLevel.Error, $"Request {requestId} finished. Response from Translate server Is Null Or Empty");
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Fatal, $"Request {requestId} finished. Error message: {ex.Message}");
                return BadRequest();
            }


            return Ok();
        }
    }
}

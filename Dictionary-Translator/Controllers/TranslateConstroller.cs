using CoreLibrary.Logger;
using Dictionary_Translator.Models;
using Dictionary_Translator.Secrets;
using Dictionary_Translator.Services.Translation;
using GoogleAPI_Library;
using GoogleAPI_Library.Exceptions;
using GoogleAPI_Library.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Dictionary_Translator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslateConstroller : Controller
    {
        readonly ILogger Logger;

        public TranslateConstroller()
        {
            Logger = new LogToFile(Environment.CurrentDirectory + "\\Logging\\log.log");
        }

        [HttpPost]
        [Route("translate")]
        public async Task<IActionResult> Translate([FromForm] string valueToTranslate)
        {
            Guid requestId = Guid.NewGuid();
            
            try
            {
                Logger.Log(LogLevel.Info, $"----------------- Request received. Id: {requestId} -------------------\nInput Data:" +
                    $"\nvalueToTranslate: {valueToTranslate}" +
                    $"\nrowIndex: {rowIndex}");

                #region GoogleSheetsManager creation
                GoogleCredentialOptions_FilePath credentials = SecretsManager.GetGoogleCredentialOptions();

                GoogleSheetsManager googleSheetsManager = new GoogleSheetsManager(credentials);
                #endregion

                if (String.IsNullOrEmpty(valueToTranslate))
                {
                    Logger.Log(LogLevel.Info, $"--- Request {requestId} finished. Input parametr is null or empty ---\n\n");
                    return BadRequest();
                }

                string privateKey = SecretsManager.GetYandexAPIKey();

                string url = @$"https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={privateKey}&lang=en-ru&text={valueToTranslate}";

                using HttpClient httpClient = new HttpClient();
                using HttpRequestMessage requestToTranslate = new HttpRequestMessage(HttpMethod.Get, url);

                var responseFromServer = await httpClient.SendAsync(requestToTranslate);
                string translatedTextJSON = responseFromServer.Content.ReadAsStringAsync().Result;

                Logger.Log(LogLevel.Info, $"{requestId} | Translation request sended");

                if (string.IsNullOrEmpty(translatedTextJSON))
                {
                    //TODO google sheets
                    Logger.Log(LogLevel.Error, $"--- Request {requestId} finished. Response from Translate server Is Null Or Empty ---\n\n");
                    
                    return BadRequest();
                }

                Root translatedText = JsonConvert.DeserializeObject<Root>(JObject.Parse(translatedTextJSON).ToString());
                Logger.Log(LogLevel.Info, $"{requestId} | Json converted to Model");

                if (translatedText is null || translatedText.def.Count == 0)
                {
                    Logger.Log(LogLevel.Error, $"--- Request {requestId} finished. Response from Translate server Is Null Or Empty ---\n\n");
                    await googleSheetsManager.Write(sheetOptions, googleSheetsNotTranslatedText);
                    return BadRequest();
                }



                Logger.Log(LogLevel.Info, $"{requestId} | Data inserted to Google Sheet");
            }
            catch (GoogleSheetsException googleEx)
            {
                Logger.Log(LogLevel.Fatal, $"--- Request {requestId} finished. Google Sheets Error: {googleEx.Message} ---\n\n");
                return BadRequest();
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Fatal, $"--- Request {requestId} finished. Error message: {ex.Message} ---\n\n");
                return BadRequest();
            }

            Logger.Log(LogLevel.Info, $"-------------Request successfully {requestId} finished------------------\n\n");
            return Ok();
        }
    }
}

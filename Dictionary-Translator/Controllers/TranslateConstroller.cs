using CoreLibrary.Logger;
using Dictionary_Translator.Models;
using Dictionary_Translator.Secrets;
using Dictionary_Translator.Services.AppSettingsManager;
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
        public async Task<IActionResult> Translate([FromForm] string valueToTranslate, [FromForm] string rowIndex)
        {
            Guid requestId = Guid.NewGuid();
            var googleSheetsNotTranslatedText = new List<IList<object>> { new object[] { "None", "None" } };

            try
            {
                Logger.Log(LogLevel.Info, $"----------------- Request received. Id: {requestId} -------------------\nInput Data:" +
                    $"\nvalueToTranslate: {valueToTranslate}" +
                    $"\nrowIndex: {rowIndex}");

                #region GoogleSheetsManager creation
                var credentials = AppSettingsManager.GetGoogleSheetsSetting(rowIndex.Replace(".0", ""));

                GoogleSheetsManager googleSheetsManager = new GoogleSheetsManager(credentials.Item1);
                GoogleSheetOptions sheetOptions = credentials.Item2;
                #endregion

                if (String.IsNullOrEmpty(valueToTranslate) || String.IsNullOrEmpty(rowIndex))
                {
                    Logger.Log(LogLevel.Info, $"--- Request {requestId} finished. Input parametr is null or empty ---\n\n");
                    return BadRequest();
                }

                string translationAPI_PrivateKey = AppSettingsManager.GetValueFromSection("YandexTranslateApiKey");

                string url = @$"https://dictionary.yandex.net/api/v1/dicservice.json/lookup?key={translationAPI_PrivateKey}&lang=en-ru&text={valueToTranslate}";

                using HttpClient httpClient = new HttpClient();
                using HttpRequestMessage requestToTranslate = new HttpRequestMessage(HttpMethod.Get, url);

                var responseFromServer = await httpClient.SendAsync(requestToTranslate);
                string translatedTextJSON = responseFromServer.Content.ReadAsStringAsync().Result;

                Logger.Log(LogLevel.Info, $"{requestId} | Translation request sended");

                if (string.IsNullOrEmpty(translatedTextJSON))
                {
                    Logger.Log(LogLevel.Error, $"--- Request {requestId} finished. Response from Translate server Is Null Or Empty ---\n\n");
                    
                    await googleSheetsManager.Write(sheetOptions, googleSheetsNotTranslatedText);
                    return BadRequest();
                }

                Root translatedTextModel = JsonConvert.DeserializeObject<Root>(JObject.Parse(translatedTextJSON).ToString());
                Logger.Log(LogLevel.Info, $"{requestId} | Json converted to Model");

                if (translatedTextModel is null || translatedTextModel.def.Count == 0)
                {
                    Logger.Log(LogLevel.Error, $"--- Request {requestId} finished. Response from Translate server Is Null Or Empty ---\n\n");
                    await googleSheetsManager.Write(sheetOptions, googleSheetsNotTranslatedText);
                    return BadRequest();
                }

                string translateTextToInsert = String.Empty;
                string translatedTextTranscription = translatedTextModel.def[0].ts ?? "null";

                List<string> translatedList =  Translation.GetTranslationsString(translatedTextModel).Take(3).ToList();
                string translatedText = Translation.GetTranslatedStringFromCollection(translatedList);
                
                var resultList = new List<IList<object>> { new object[] { translatedText, translatedTextTranscription } };
                await googleSheetsManager.Write(sheetOptions, resultList);

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

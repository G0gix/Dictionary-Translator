using Microsoft.AspNetCore.Mvc;

namespace Dictionary_Translator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TranslateConstroller : Controller
    {
        [HttpPost]
        [Route("translate")]
        public IActionResult Translate([FromForm] string valueToTranslate)
        {
            string data = valueToTranslate;
            return Ok();
        }
    }
}

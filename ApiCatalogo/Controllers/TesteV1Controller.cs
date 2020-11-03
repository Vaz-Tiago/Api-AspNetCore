using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    //[ApiVersion("1.0", Deprecated = true)] -> Envia no header que está obsoleto
    [ApiVersion("1.0")]
    //[ApiVersion("2.0")] // Controlador pode atender duas versões -> Mas deve inicar qual método atende qual
    //[Route("api/{v:apiVersion}/teste")]
    [Route("api/teste")]

    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h1>Teste V1 Controller - V 1.0 </h1></body></html>", "text/html");
        }

        //[HttpGet, MapToApiVersion("2.0")] // Não é recomendado -> Versionar sempre em arquivos diferentes. Isso pode virar uma bola de neve
        //public IActionResult GetVersao2()
        //{
        //    return Content("<html><body><h1>Teste V1 Controller - V 2.0 </h1></body></html>", "text/html");
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiCatalogo.Controllers
{
    [ApiVersion("2.0")]
    //[Route("api/v{v:apiVersion}/teste")] -> Removido pois vai acessar via header
    [Route("api/teste")]
    [ApiController]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h1>Teste V2 Controller - V 2.0 </h1></body></html>", "text/html");
        }
    }
}

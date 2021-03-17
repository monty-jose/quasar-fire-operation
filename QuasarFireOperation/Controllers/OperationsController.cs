using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuasarFireOperation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        [HttpPost]
        public ActionResult topSecret(Models.Satellite satellite)
        {
            return Ok("hola");
        }

        [HttpPost]
        public ActionResult topSecret_Split(Models.Satellite satellite)
        {
            return Ok("Post Top Secrete");
        }

        [HttpGet]
        public ActionResult topSecret_Split()
        {
            return Ok("get Stop Secret Split");
        }

    }
}

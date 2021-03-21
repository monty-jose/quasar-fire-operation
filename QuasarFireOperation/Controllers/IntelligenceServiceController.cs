using Microsoft.AspNetCore.Mvc;
using QuasarFireOperation.Entities;
using QuasarFireOperation.Models;
using QuasarFireOperation.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuasarFireOperation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntelligenceServiceController : Controller
    {
        private readonly OperationsService operationsService;

        public IntelligenceServiceController(OperationsService operationsService)
        {
            this.operationsService = operationsService;
        }
        
        [HttpPost("topSecret")]
        public ActionResult<ResponseDTO> Post([FromBody] SatellitesListDTO requestSatelliteList)
        {
            if (requestSatelliteList == null)
            {
                return NotFound();
            }

            ResultDTO data = operationsService.TopSecretResponse(requestSatelliteList);

            if (data.result)
            {
                return Ok(data.response);
            }
            else
            {
                return NotFound();
            }

            data.response.position.x = 150;
            data.response.position.x = 80;
            data.response.message = "Vamos funciono";
            return Ok(data.response);
        }

        [HttpPost("topSecret_Split")]
        public ActionResult topSecret_Split([FromBody] SatelliteMessageDTO satellite)
        {
            return Ok("Post Top Secrete");
            //return NotFound();
        }
        
        [HttpGet("topSecret_Split")]
        public ActionResult Get()
        {
            ResponseDTO messageResponse = new ResponseDTO();

            try
            {
                messageResponse = operationsService.MessageResponse();

                if (messageResponse != null)
                {
                    return Ok(messageResponse);
                }
                else
                {
                    return NotFound();
                }
                
            }
            catch (System.Exception ex)
            {
                return NotFound(ex.Message);
                throw;
            }           
        }

        [HttpGet("GetMessage")]
        public IEnumerable<MessagesSecret> GetMessage ()
        {
            return operationsService.Message();
        }

        [HttpGet("GetTest")]
        public string GetTest()
        {
            return "Hola entro";
        }

    }
}

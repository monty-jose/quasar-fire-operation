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
        public ActionResult<ResponseDTO> Post([FromBody] List<SatelliteDTO> satellite)
        {
            ResponseDTO messageResponse = new ResponseDTO();
            if (satellite == null)
            {
                return NotFound();
            }

            messageResponse.position.x = 150;
            messageResponse.position.x = 80;
            messageResponse.message = "Vamos funciono";
            return Ok(messageResponse);
        }

        [HttpPost("topSecret_Split")]
        public ActionResult topSecret_Split([FromBody] SatelliteDTO satellite)
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

    }
}

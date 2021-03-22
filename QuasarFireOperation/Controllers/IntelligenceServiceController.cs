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
        public ActionResult<ResponseDTO> Post([FromBody] TopSecretRequestDTO requestSatelliteList)
        {
            if (requestSatelliteList != null)
            {
                ResultDTO data = operationsService.TopSecretResponse(requestSatelliteList.satellites);
                
                if (!data.error)
                {
                    return Ok(data.response);
                }
                else
                {
                    return NotFound();
                }
            }
            else
                return NotFound();

        }

        [HttpPost("topSecret_Split/{satellite_name}")]
        public ActionResult topSecret_Split([FromBody] TopSecretSplitRequestDTO requestSatellite, string satellite_name)
        {
            return Ok("Post Top Secrete Split: "+ satellite_name);
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

        [HttpGet("GetTest")]
        public string GetTest()
        {
            return "Hola entro";
        }

    }
}

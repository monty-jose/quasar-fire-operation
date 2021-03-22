using Microsoft.AspNetCore.Mvc;
using QuasarFireOperation.Entities;
using QuasarFireOperation.Models;
using QuasarFireOperation.Services;
using System;
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
                if (operationsService.IsValidRequest(requestSatelliteList.satellites))
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
            else
                return NotFound();

        }

        [HttpPost("topSecret_Split/{satellite_name}")]
        public ActionResult TopSecret_Split([FromBody] TopSecretSplitRequestDTO requestSatellite, string satellite_name)
        {
            if (requestSatellite != null && !String.IsNullOrEmpty(satellite_name))
            {
                ResultDTO data = operationsService.TopSecretSplitPost(requestSatellite, satellite_name);

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
        
        [HttpGet("topSecret_Split")]
        public ActionResult TopSecret_SplitGet()
        {
            ResultDTO data = operationsService.TopSecretSplitGet();

            if (!data.error)
            {
                return Ok(data.response);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetTest")]
        public string GetTest()
        {
            return "Hola entro";
        }

    }
}

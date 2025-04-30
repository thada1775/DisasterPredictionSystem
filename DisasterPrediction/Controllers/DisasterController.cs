using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DisasterPrediction.API.Controllers
{
    [ApiController]
    [Route("api/disaster-risks")]
    public class DisasterController : ControllerBase
    {
        private readonly IDisasterService _service;
        public DisasterController(IDisasterService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var result = await _service.GetDisasterRisk();
            return Ok(result);
        }
    }
}

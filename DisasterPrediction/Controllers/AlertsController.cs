using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DisasterPrediction.API.Controllers
{
    [ApiController]
    [Route("api/alerts")]
    public class AlertsController : ControllerBase
    {
        private readonly IAlertsService _service;
        public AlertsController(IAlertsService service)
        {
            _service = service;
        }

        [HttpPost("summary")]
        public async Task<IActionResult> FindSummary(AlertHistoryFilterDto filterDto)
        {
            var result = await _service.FindSummaryHistoryAsync(filterDto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetHistoryByRegion(string id)
        {
            var result = await _service.GetHistoryByRegionAsync(id);
            return Ok(result);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendAlert()
        {
            await _service.SendWarningMessage();
            return Ok();
        }
    }
}

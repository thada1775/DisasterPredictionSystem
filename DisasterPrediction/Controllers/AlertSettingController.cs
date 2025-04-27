using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DisasterPrediction.API.Controllers
{
    [ApiController]
    [Route("api/alert")]
    public class AlertSettingController : ControllerBase
    {
        private readonly IAlertSettingService _service;
        public AlertSettingController(IAlertSettingService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(AlertSettingDto request)
        {
            var result = await _service.CreateSettingAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(RegionDto request)
        {
            var updatedTask = await _service.UpdateEntityAsync(request);
            return Ok(updatedTask);
        }

        [HttpDelete]
        public async Task Delete(string id)
        {
            await _service.DeleteSettingAsync(id);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.GetSettingAsync(id);
            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> Find()
        {
            var result = await _service.FindSettingAsync();
            return Ok(result);
        }
    }
}

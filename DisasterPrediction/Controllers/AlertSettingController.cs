using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DisasterPrediction.API.Controllers
{
    [ApiController]
    [Route("api/alertSetting")]
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
        public async Task<IActionResult> Update(AlertSettingDto request)
        {
            var updatedTask = await _service.UpdateSettingAsync(request);
            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _service.DeleteSettingAsync(id);
        }

        [HttpGet("{id}")]
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

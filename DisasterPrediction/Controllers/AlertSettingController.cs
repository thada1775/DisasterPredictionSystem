using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisasterPrediction.API.Controllers
{
    [ApiController]
    [Route("api/alertSetting")]
    [Authorize(Roles ="Admin")]
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
        public async Task Delete(string id, string disasterType)
        {
            await _service.DeleteSettingAsync(id, disasterType);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id,string disasterType)
        {
            var result = await _service.GetSettingAsync(id, disasterType);
            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> Find(AlertSettingFilterDto filterDto)
        {
            var result = await _service.FindSettingAsync(filterDto);
            return Ok(result);
        }
    }
}

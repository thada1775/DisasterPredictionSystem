using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
using DisasterPrediction.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisasterPrediction.API.Controllers
{
    [ApiController]
    [Route("api/region")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _service;
        public RegionController(IRegionService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegionDto request)
        {
            var result = await _service.CreateEntityAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(RegionDto request)
        {
            var updatedTask = await _service.UpdateEntityAsync(request);
            return Ok(updatedTask);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _service.DeleteEntityAsync(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _service.GetEntityAsync(id);
            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> Find()
        {
            var result = await _service.FindEntityuAsync();
            return Ok(result);
        }

    }
}

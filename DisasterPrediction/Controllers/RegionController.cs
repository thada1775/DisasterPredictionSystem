using DisasterPrediction.Application.DTOs;
using DisasterPrediction.Application.Interfaces;
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
            var result = await _service.CreateAsync(request);
            return Ok(result);
        }

    }
}

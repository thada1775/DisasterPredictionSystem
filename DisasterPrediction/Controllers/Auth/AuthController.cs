using DisasterPrediction.Application.Common.Interfaces.Auth;
using DisasterPrediction.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DisasterPrediction.API.Controllers.Auth
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var token = await _authService.RegisterAsync(model);
            if (token == null) return BadRequest(new { message = "Registration failed" });

            return Ok(new { Token = token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var token = await _authService.LoginAsync(model);
            if (token == null) return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new { Token = token });
        }
    }
}

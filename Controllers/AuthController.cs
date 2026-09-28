using Microsoft.AspNetCore.Mvc;
using TaskManagementApi.DTOs.Auth;
using TaskManagementApi.Services.Interfaces;

namespace TaskManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

  

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(
            LoginRequestDto request)
        {
            var user = await _authService.LoginAsync(request);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid login ID or password."
                });
            }

            return Ok(user);
        }
    }
}
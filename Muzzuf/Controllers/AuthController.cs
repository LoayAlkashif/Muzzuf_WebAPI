using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Muzzuf.Service.DTO.AuthDTO;
using Muzzuf.Service.IService;

namespace Muzzuf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto dto)
        {
            await _authService.Register(dto);
            return Ok(new
            {
                Message = "Registration Successful, Please Check your email to verify your account"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.Login(dto);
            return Ok(new
            {
                token
            });
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            await _authService.ConfirmEmailAsync(userId, token);
            return Ok(new
            {
                Message = "Email Verified Successfuly."
            });
        }
    }
}

using AuthApi.DTOs;
using AuthApi.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
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
        public async Task<IActionResult> Register(UserSIgnUpDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.RegisterUserAsync(request);
            if (!response.success)
            {
                return BadRequest(response.message);
            }
            return Ok(response.message);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.LoginUserAsync(request);
            if (!response.success)
            {
                return BadRequest(response.message);
            }
            return Ok(response.token);
        }


    }

}

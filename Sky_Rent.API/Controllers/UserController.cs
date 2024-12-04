using Microsoft.AspNetCore.Mvc;
using Sky_Rent.Application.DTOs;
using Sky_Rent.Application.Services;

namespace Sky_Rent.API.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthenticationService _authenticationService;
        public UserController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegistrationDTO registrationDto)
        {
            try
            {
                var userId = await _authenticationService.RegisterUserAsync(registrationDto);
                return Ok(new { UserId = userId });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDTO loginDto)
        {
            try
            {
                var token = await _authenticationService.LoginUserAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}

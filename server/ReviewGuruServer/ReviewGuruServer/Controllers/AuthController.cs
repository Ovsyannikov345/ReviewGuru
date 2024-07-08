using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;

namespace ReviewGuru.API.Controllers
{
   
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginDto authData, CancellationToken cancellationToken = default)
        {
            var tokens = await _authService.LoginAsync(authData, cancellationToken);


            return Ok(tokens);
        }


        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegisterDto userData, CancellationToken cancellationToken = default)
        {
            var tokens = await _authService.RegisterAsync(userData, cancellationToken);


            return Ok(tokens);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync(LogoutDto logoutData, CancellationToken cancellationToken = default)
        {
            await _authService.LogoutAsync(logoutData, cancellationToken);

            return Ok();
        }

        [HttpPost("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyUserAsync(VerifyAccountDto verificationData, CancellationToken cancellationToken = default)
        {
            await _authService.VerifyUserAsync(verificationData, cancellationToken);


            return Ok();
        }
    }
}

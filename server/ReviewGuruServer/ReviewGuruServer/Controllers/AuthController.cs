//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using ReviewGuru.BLL.DTOs;
//using ReviewGuru.BLL.Services.IServices;

//namespace ReviewGuru.API.Controllers
//{
    
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController(IAuthService authService) : ControllerBase
//    {
//        private readonly IAuthService _authService = authService;

//        [HttpPost("login")]
//        [AllowAnonymous]
//        public async Task<IActionResult> LoginAsync(LoginDto authData)
//        {
//            var tokens = await _authService.LoginAsync(authData);

//            return Ok(tokens);
//        }

//        [HttpPost("register")]
//        [AllowAnonymous]
//        public async Task<IActionResult> RegisterAsync(RegisterDto userData)
//        {
//            var tokens = await _authService.RegisterAsync(userData);

//            return Ok(tokens);
//        }

//        [HttpPost("logout")]
//        public async Task<IActionResult> LogoutAsync(LogoutDto logoutData)
//        {
//            await _authService.LogoutAsync(logoutData);

//            return Ok();
//        }
//    }
//}

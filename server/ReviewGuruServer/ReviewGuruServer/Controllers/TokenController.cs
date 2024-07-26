using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;

namespace ReviewGuru.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController(ITokenService tokenService) : ControllerBase
    {
        private readonly ITokenService _tokenService = tokenService;

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshTokensAsync(RefreshTokensDto refreshData, CancellationToken cancellationToken = default)
        {
            var tokens = await _tokenService.RefreshTokensAsync(refreshData, cancellationToken);

            return Ok(tokens);
        }
    }
}

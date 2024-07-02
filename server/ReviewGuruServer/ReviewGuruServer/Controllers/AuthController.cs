using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewGuruServer.Database;
using ReviewGuruServer.DataTransferObjects;

namespace ReviewGuruServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ReviewGuruDbContext _context;

        public AuthController(ReviewGuruDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthDto authData)
        {
            throw new NotImplementedException();
        }
    }
}

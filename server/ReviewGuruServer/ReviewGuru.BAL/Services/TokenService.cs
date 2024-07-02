using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class TokenService : ITokenService
    {
        public Task<TokenDto> CreateTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDto> RefreshTokenAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}

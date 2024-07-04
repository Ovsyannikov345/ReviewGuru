using ReviewGuru.BLL.DTOs;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims, DateTime expirationDate, string secretKey);

        Task<TokenDto> CreateTokensAsync(User user);

        Task<TokenDto> RefreshTokensAsync(RefreshTokensDto refreshData);

        public Task<int> RemoveRefreshTokenAsync(string refreshToken);
    }
}

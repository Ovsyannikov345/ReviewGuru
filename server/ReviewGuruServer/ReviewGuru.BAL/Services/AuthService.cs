using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class AuthService : IAuthService
    {
        public Task<TokenDto> LoginAsync(LoginDto authData)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LogoutAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<TokenDto> RegisterAsync(RegisterDto userData)
        {
            throw new NotImplementedException();
        }
    }
}

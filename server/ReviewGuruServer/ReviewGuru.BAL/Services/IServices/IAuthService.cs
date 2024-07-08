using ReviewGuru.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto authData, CancellationToken cancellationToken = default);

        Task<TokenDto> RegisterAsync(RegisterDto userData, CancellationToken cancellationToken = default);

        Task LogoutAsync(LogoutDto logoutData, CancellationToken cancellationToken = default);

        Task VerifyUserAsync(VerifyAccountDto verificationData, CancellationToken cancellationToken = default);
    }
}

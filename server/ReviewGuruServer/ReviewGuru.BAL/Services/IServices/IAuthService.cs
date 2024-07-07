﻿using ReviewGuru.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{
    public interface IAuthService
    {
        Task<TokenDto> LoginAsync(LoginDto authData);

        Task<TokenDto> RegisterAsync(RegisterDto userData);

        Task LogoutAsync(LogoutDto logoutData);

        Task VerifyUserAsync(VerifyAccountDto verificationData);
    }
}

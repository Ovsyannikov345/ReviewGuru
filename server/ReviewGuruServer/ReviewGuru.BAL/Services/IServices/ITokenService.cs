﻿using Microsoft.IdentityModel.Tokens;
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
        public string GenerateToken(IEnumerable<Claim> claims, DateTime expirationDate, string secretKey);

        public Task<TokenDto> CreateTokensAsync(User user, CancellationToken cancellationToken = default);

        public Task<TokenDto> RefreshTokensAsync(RefreshTokensDto refreshData, CancellationToken cancellationToken = default);

        public string GenerateVerificationToken(User user);

        public Task<TokenValidationResult> ValidateVerificationTokenAsync(string verificationToken, CancellationToken cancellationToken = default);

        public Task<int> RemoveRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}

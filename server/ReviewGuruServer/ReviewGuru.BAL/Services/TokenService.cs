using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class TokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository) : ITokenService
    {
        private readonly IConfiguration _configuration = configuration;

        private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;

        public async Task<TokenDto> CreateTokensAsync(User user)
        {
            ValidateJwtConfigData();

            var claims = GetClaims(user);

            var accessToken = GenerateToken(claims,
                DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:AccessMinutesExpire"]!)),
                _configuration["Jwt:AccessSecretKey"]!);

            var refreshToken = GenerateToken(claims,
                DateTime.Now.AddDays(int.Parse(_configuration["Jwt:RefreshDaysExpire"]!)),
                _configuration["Jwt:RefreshSecretKey"]!);

            try
            {
                await _refreshTokenRepository.AddAsync(new RefreshToken() { Token = refreshToken });
            }
            catch (Exception)
            {
                throw new InternalServerErrorException("Error while saving new refresh token");
            }


            return new TokenDto(accessToken, refreshToken);
        }

        public async Task<TokenDto> RefreshTokensAsync(RefreshTokensDto refreshData)
        {
            ValidateJwtConfigData();

            var refreshToken = await _refreshTokenRepository.GetAsync(token => token.Token == refreshData.RefreshToken);

            if (refreshToken == null)
            {
                throw new NotFoundException("Provided refresh token is not found");
            }

            var tokenHandler = new JsonWebTokenHandler();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:RefreshSecretKey"]!)),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudiences = _configuration.GetSection("Jwt:Audiences").Get<string[]>(),
                ValidateLifetime = true,
            };

            TokenValidationResult validationResult = await tokenHandler.ValidateTokenAsync(refreshToken.Token, tokenValidationParameters);

            if (!validationResult.IsValid)
            {
                throw new ForbiddenException("Provided refresh token is invalid");
            }

            string newAccessToken = GenerateToken(validationResult.ClaimsIdentity.Claims,
                DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:AccessMinutesExpire"]!)),
                _configuration["Jwt:AccessSecretKey"]!);

            string newRefreshToken = GenerateToken(validationResult.ClaimsIdentity.Claims,
                DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:RefreshDaysExpire"]!)),
                _configuration["Jwt:RefreshSecretKey"]!);

            try
            {
                await _refreshTokenRepository.DeleteAsync(refreshToken);
                await _refreshTokenRepository.AddAsync(new RefreshToken() { Token = newRefreshToken });
            }
            catch (Exception)
            {
                throw new InternalServerErrorException("Error while changing refresh token in database");
            }

            return new TokenDto(newAccessToken, newRefreshToken);
        }

        public string GenerateToken(IEnumerable<Claim> claims, DateTime expirationDate, string secretKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationDate,
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration.GetSection("Jwt:Audiences:0").Value,
                SigningCredentials = credentials
            };

            var tokenHandler = new JsonWebTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return token;
        }

        public async Task<int> RemoveRefreshTokenAsync(string refreshToken)
        {
            var token = await _refreshTokenRepository.GetAsync(t => t.Token == refreshToken);

            if (token == null)
            {
                throw new NotFoundException("Provided refresh token is not found");
            }

            return await _refreshTokenRepository.DeleteAsync(token);
        }

        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new("Id", user.UserId.ToString()),
                new(ClaimTypes.Role, Roles.User.ToString())
            };

            return claims;
        }

        private void ValidateJwtConfigData()
        {
            if (_configuration["Jwt:AccessSecretKey"] == null)
            {
                throw new InternalServerErrorException("Access secret key not defined");
            }

            if (_configuration["Jwt:RefreshSecretKey"] == null)
            {
                throw new InternalServerErrorException("Refresh secret key not defined");
            }

            if (_configuration["Jwt:AccessMinutesExpire"] == null || !int.TryParse(_configuration["Jwt:AccessMinutesExpire"], out _))
            {
                throw new InternalServerErrorException("Access token expiration not defined");
            }

            if (_configuration["Jwt:RefreshDaysExpire"] == null || !int.TryParse(_configuration["Jwt:RefreshDaysExpire"], out _))
            {
                throw new InternalServerErrorException("Refresh token expiration not defined");
            }
        }
    }
}

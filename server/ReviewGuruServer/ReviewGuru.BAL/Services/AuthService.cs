using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.BLL.Utilities.EmailSender;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.BLL.Utilities.Validators;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class AuthService(
        IConfiguration configuration,
        IUserRepository userRepository,
        ITokenService tokenService,
        IMapper mapper,
        IEmailSender emailSender,
        ILogger logger,
        RegistrationValidator registrationValidator) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;

        private readonly IUserRepository _userRepository = userRepository;

        private readonly ITokenService _tokenService = tokenService;

        private readonly IMapper _mapper = mapper;

        private readonly IEmailSender _emailSender = emailSender;

        private readonly ILogger _logger;

        private readonly RegistrationValidator _registrationValidator = registrationValidator;

        public async Task<TokenDto> LoginAsync(LoginDto authData, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByItemAsync(u => u.Login == authData.Login, cancellationToken);

            if (user == null)
            {
                _logger.Warning($"User with login {authData.Login} is not found");
                throw new NotFoundException($"User with login {authData.Login} is not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(authData.Password, user.Password))
            {
                _logger.Warning($"Invalid credentials for user {user.Login}");
                throw new UnauthorizedException("Provided credentials are invalid");
            }

            _logger.Information($"User {user.Login} logged in successfully");

            return await _tokenService.CreateTokensAsync(user, cancellationToken);
        }

        public async Task LogoutAsync(LogoutDto logoutData, CancellationToken cancellationToken = default)
        {
            await _tokenService.RemoveRefreshTokenAsync(logoutData.RefreshToken, cancellationToken);
            _logger.Information($"User logged out successfully");
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto userData, CancellationToken cancellationToken = default)
        {
            var validationResult = await _registrationValidator.ValidateAsync(userData, cancellationToken);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException($"Registration error. {validationResult.Errors[0].ErrorMessage}");
            }

            User user = _mapper.Map<User>(userData);

            bool isLoginAvailable = await IsLoginAvailable(user.Login, cancellationToken);

            if (!isLoginAvailable)
            {
                throw new BadRequestException("Login is taken");
            }

            bool isEmailAvailable = await IsEmailAvailable(user.Email, cancellationToken);

            if (!isEmailAvailable)
            {
                throw new BadRequestException("Email is taken");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(userData.Password);

            User createdUser = await _userRepository.AddAsync(user, cancellationToken);

            string verificationToken = _tokenService.GenerateVerificationToken(createdUser);

            try
            {
                await _emailSender.SendEmailAsync(createdUser.Email,
                    "Email verification",
                    EmailMessages.GetVerificationMessage($"{_configuration["Verification:URL"]}?token={verificationToken}"),
                    cancellationToken);
            }
            catch (Exception)
            {
                await _userRepository.DeleteAsync(createdUser.UserId, cancellationToken);
                throw;
            }

            _logger.Information($"User {createdUser.Login} registered successfully");

            return await _tokenService.CreateTokensAsync(createdUser, cancellationToken);
        }

        public async Task VerifyUserAsync(VerifyAccountDto verificationData, CancellationToken cancellationToken = default)
        {
            TokenValidationResult validationResult = await _tokenService.ValidateVerificationTokenAsync(verificationData.VerificationToken, cancellationToken);

            var userIdClaim = validationResult.ClaimsIdentity.FindFirst("Id") ?? throw new InternalServerErrorException("Verification failed. User id is not provided");

            int userId = int.Parse(userIdClaim.Value);

            User user = await _userRepository.GetByItemAsync(u => u.UserId == userId, cancellationToken) ?? throw new NotFoundException("User to verify is not found");

            if (user.IsVerified)
            {
                throw new BadRequestException("User has already been verified");
            }

            user.IsVerified = true;
            await _userRepository.UpdateAsync(user, cancellationToken);

            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Thank you for joining our media review service!", EmailMessages.WelcomeMessage, cancellationToken);
                _logger.Information($"User {user.Login} verified successfully");
            }
            catch (InternalServerErrorException ex)
            {
                _logger.Error($"Error during user verification: {ex.Message}");
                throw;
            }
        }

        private async Task<bool> IsEmailAvailable(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByItemAsync(u => u.Email == email, cancellationToken);

            return user == null;
        }

        private async Task<bool> IsLoginAvailable(string login, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetByItemAsync(u => u.Login == login, cancellationToken);

            return user == null;
        }
    }
}

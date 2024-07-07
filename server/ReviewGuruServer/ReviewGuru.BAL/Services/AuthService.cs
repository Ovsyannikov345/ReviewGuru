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
        RegistrationValidator registrationValidator) : IAuthService
    {
        private readonly IConfiguration _configuration = configuration;

        private readonly IUserRepository _userRepository = userRepository;

        private readonly ITokenService _tokenService = tokenService;

        private readonly IMapper _mapper = mapper;

        private readonly IEmailSender _emailSender = emailSender;

        private readonly RegistrationValidator _registrationValidator = registrationValidator;

        public async Task<TokenDto> LoginAsync(LoginDto authData)
        {
            var user = await _userRepository.GetAsync(u => u.Login == authData.Login);

            if (user == null)
            {
                throw new NotFoundException($"User with login {authData.Login} is not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(authData.Password, user.Password))
            {
                throw new UnauthorizedException("Provided credentials are invalid");
            }

            return await _tokenService.CreateTokensAsync(user);
        }

        public async Task LogoutAsync(LogoutDto logoutData)
        {
            await _tokenService.RemoveRefreshTokenAsync(logoutData.RefreshToken);
        }

        public async Task<TokenDto> RegisterAsync(RegisterDto userData)
        {
            var validationResult = await _registrationValidator.ValidateAsync(userData);

            if (!validationResult.IsValid)
            {
                throw new BadRequestException($"Registration error. {validationResult.Errors[0].ErrorMessage}");
            }

            User user = _mapper.Map<User>(userData);

            bool isLoginAvailable = await IsLoginAvailable(user.Login);

            if (!isLoginAvailable)
            {
                throw new BadRequestException("Login is taken");
            }

            bool isEmailAvailable = await IsEmailAvailable(user.Email);

            if (!isEmailAvailable)
            {
                throw new BadRequestException("Email is taken");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(userData.Password);

            User createdUser = await _userRepository.AddAsync(user);

            string verificationToken = _tokenService.GenerateVerificationToken(createdUser);

            try
            {
                await _emailSender.SendEmailAsync(createdUser.Email,
                    "Email verification",
                    EmailMessages.GetVerificationMessage($"{_configuration["Verification:URL"]}?token={verificationToken}"));
            }
            catch (Exception)
            {
                await _userRepository.DeleteAsync(createdUser);
                throw;
            }

            return await _tokenService.CreateTokensAsync(createdUser);
        }

        public async Task VerifyUserAsync(VerifyAccountDto verificationData)
        {
            TokenValidationResult validationResult = await _tokenService.ValidateVerificationTokenAsync(verificationData.VerificationToken);

            var userIdClaim = validationResult.ClaimsIdentity.FindFirst("Id") ?? throw new InternalServerErrorException("Verification failed. User id is not provided");

            int userId = int.Parse(userIdClaim.Value);

            User user = await _userRepository.GetAsync(u => u.UserId == userId) ?? throw new NotFoundException("User to verify is not found");

            if (user.IsVerified)
            {
                throw new BadRequestException("User has already been verified");
            }

            user.IsVerified = true;
            await _userRepository.UpdateAsync(user);

            try
            {
                await _emailSender.SendEmailAsync(user.Email, "Thank you for joining our media review service!", EmailMessages.WelcomeMessage);
            }
            catch (InternalServerErrorException)
            {
                // Ignored because welcome email sending is optional. TODO add logging here.
            }
        }

        private async Task<bool> IsEmailAvailable(string email)
        {
            var user = await _userRepository.GetAsync(u => u.Email == email);

            return user == null;
        }

        private async Task<bool> IsLoginAvailable(string login)
        {
            var user = await _userRepository.GetAsync(u => u.Login == login);

            return user == null;
        }
    }
}

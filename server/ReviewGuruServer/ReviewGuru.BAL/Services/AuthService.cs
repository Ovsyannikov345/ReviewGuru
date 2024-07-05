using AutoMapper;
using BCrypt.Net;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.BLL.Utilities.Validators;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class AuthService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper, RegistrationValidator registrationValidator) : IAuthService
    {
        private readonly IUserRepository _userRepository = userRepository;

        private readonly ITokenService _tokenService = tokenService;

        private readonly IMapper _mapper = mapper;

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

            return await _tokenService.CreateTokensAsync(createdUser);
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

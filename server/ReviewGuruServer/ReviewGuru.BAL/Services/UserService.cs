using AutoMapper;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<Media>> GetUserFavoritesAsync(int userId, CancellationToken cancellationToken = default)
        {
            User user = await _userRepository.GetUserWithFavoritesAsync(u => u.UserId == userId, cancellationToken) ??
                        throw new NotFoundException($"User with id {userId} is not found");

            return user.Favorites;
        }
    }
}

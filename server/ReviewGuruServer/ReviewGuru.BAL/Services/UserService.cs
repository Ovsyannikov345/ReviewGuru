using AutoMapper;
using Serilog;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class UserService(IUserRepository userRepository, ILogger logger) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        private readonly ILogger _logger = logger;

        public async Task<IEnumerable<Media>> GetUserFavoritesAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.MaxPageSize,
            string searchText = "",
            string mediaType = "",
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Media, bool>> filter = (media) =>
                       (mediaType == "" || media.MediaType == mediaType) &&
                       (media.Name.Contains(searchText) ||
                       media.Authors.Any(author => (author.FirstName + " " + author.LastName).Contains(searchText)));

            IEnumerable<Media>? favorites = await _userRepository.GetUserFavoritesAsync(userId, pageNumber, pageSize, filter, cancellationToken);

            if (favorites == null)
            {
                _logger.Error("User with id {0} is not found", userId);

                throw new NotFoundException($"User with id {userId} is not found");
            }

            _logger.Information("Favorites were returned");

            return favorites;
        }

        public async Task<int> GetUserFavoritesCountAsync(
            int userId,
            string searchText = "",
            string mediaType = "",
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Media, bool>> filter = (media) =>
                       (mediaType == "" || media.MediaType == mediaType) &&
                       (media.Name.Contains(searchText) ||
                       media.Authors.Any(author => (author.FirstName + " " + author.LastName).Contains(searchText)));

            _logger.Information("Favorites count were returned");

            return await _userRepository.GetUserFavoritesCountAsync(userId, filter, cancellationToken);
        }
    }
}

using AutoMapper;
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
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<Media>> GetUserFavoritesAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Media, bool>> filter = (media) =>
                       (mediaType == "" || media.MediaType == mediaType) &&
                       (media.Name.Contains(searchText) ||
                       media.Authors.Any(author => (author.FirstName + " " + author.LastName).Contains(searchText)));

            IEnumerable<Media> favorites = await _userRepository.GetUserFavoritesAsync(userId, pageNumber, pageSize, filter, cancellationToken) ??
                                           throw new NotFoundException($"User with id {userId} is not found");

            return favorites;
        }
    }
}

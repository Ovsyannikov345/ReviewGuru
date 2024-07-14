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
    public class MediaService(IMediaRepository mediaRepository, IUserRepository userRepository) : IMediaService
    {
        private readonly IMediaRepository _mediaRepository = mediaRepository;

        private readonly IUserRepository _userRepository = userRepository;

        public async Task<IEnumerable<Media>> GetMediaListAsync(
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


            return await _mediaRepository.GetAllAsync(pageNumber, pageSize, filter, cancellationToken: cancellationToken);
        }

        public async Task<int> GetMediaCountAsync(string searchText = "", string mediaType = "", CancellationToken cancellationToken = default)
        {
            Expression<Func<Media, bool>> filter = (media) =>
                       (mediaType == "" || media.MediaType == mediaType) &&
                       (media.Name.Contains(searchText) ||
                       media.Authors.Any(author => (author.FirstName + " " + author.LastName).Contains(searchText)));

            return await _mediaRepository.CountAsync(filter, cancellationToken);
        }

        public async Task AddMediaToFavoritesAsync(int userId, int mediaId, CancellationToken cancellationToken = default)
        {
            User user = await _userRepository.GetUserWithFavoritesAsync(u => u.UserId == userId, cancellationToken) ??
                        throw new NotFoundException($"User with id {userId} was not found");

            if (user.Favorites.Any(media => media.MediaId == mediaId))
            {
                throw new BadRequestException("Media is already in favorites");
            }

            Media media = await _mediaRepository.GetByItemAsync(m => m.MediaId == mediaId, cancellationToken) ??
                          throw new NotFoundException($"Media with id {mediaId} was not found");

            user.Favorites.Add(media);

            await _userRepository.UpdateAsync(user, cancellationToken);
        }
    }
}


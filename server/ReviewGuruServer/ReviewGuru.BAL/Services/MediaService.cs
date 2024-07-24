using AutoMapper;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class MediaService(
        IMediaRepository mediaRepository, 
        IAuthorRepository authorRepository, 
        IUserRepository userRepository, 
        IMapper mapper,
        ILogger logger
        ) : IMediaService
    {
        private readonly IMediaRepository _mediaRepository = mediaRepository;

        private readonly IAuthorRepository _authorRepository = authorRepository;

        private readonly IUserRepository _userRepository = userRepository;

        private readonly IMapper _mapper;

        private readonly ILogger _logger = logger;

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

            _logger.Information($"Page {pageNumber} of media has been returned");

            return await _mediaRepository.GetAllAsync(pageNumber, pageSize, filter, cancellationToken: cancellationToken);
        }

        public async Task<int> GetMediaCountAsync(string searchText = "", string mediaType = "", CancellationToken cancellationToken = default)
        {
            Expression<Func<Media, bool>> filter = (media) =>
                       (mediaType == "" || media.MediaType == mediaType) &&
                       (media.Name.Contains(searchText) ||
                       media.Authors.Any(author => (author.FirstName + " " + author.LastName).Contains(searchText)));

            _logger.Information("Count of media has been returned");

            return await _mediaRepository.CountAsync(filter, cancellationToken);
        }

        public async Task AddMediaToFavoritesAsync(int userId, int mediaId, CancellationToken cancellationToken = default)
        {
            User user = await _userRepository.GetUserWithFavoritesAsync(u => u.UserId == userId, cancellationToken);
            if (user == null)
            {
                _logger.Error($"User with id {userId} was not found");
                throw new NotFoundException($"User with id {userId} was not found");
            }

            if (user.Favorites.Any(media => media.MediaId == mediaId))
            {
                _logger.Error("Media is already in favorites");
                throw new BadRequestException("Media is already in favorites");
            }

            Media media = await _mediaRepository.GetByItemAsync(m => m.MediaId == mediaId, cancellationToken);
            if (media == null)
            {
                _logger.Error($"Media with id {mediaId} was not found");
                throw new NotFoundException($"Media with id {mediaId} was not found");
            }

            user.Favorites.Add(media);

            _logger.Information("Media has been added to favorites");

            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task RemoveMediaFromFavoritesAsync(int userId, int mediaId, CancellationToken cancellationToken = default)
        {
            User? user = await _userRepository.GetUserWithFavoritesAsync(u => u.UserId == userId, cancellationToken);

            if (user == null)
            {
                _logger.Error("User with id {UserId} was not found", userId);

                throw new NotFoundException($"User with id {userId} was not found");
            }

            Media? media = user.Favorites.FirstOrDefault(m => m.MediaId == mediaId);

            if (media == null)
            {
                _logger.Error("Media with id {MediaId} was not found in user with id {UserId} favorites", mediaId, userId);

                throw new BadRequestException("Media is not in favorites");
            }

            user.Favorites.Remove(media);
            _logger.Information("Media has been removed from favorites");
            await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task AddMediaAsync(MediaToCreateDTO mediaDto, CancellationToken cancellationToken = default)
        {
            var authors = await CheckAndAddAuthors(mediaDto.AuthorsToCreateDTO, cancellationToken);

            var media = await CheckAndAddMedia(mediaDto, authors, cancellationToken);
        }

        private async Task<ICollection<Author>> CheckAndAddAuthors(ICollection<AuthorToCreateDTO> authorDtos, CancellationToken cancellationToken)
        {
            var authors = new List<Author>();

            foreach (var authorDto in authorDtos)
            {
                var existingAuthor = await _authorRepository.GetByItemAsync(a => a.FirstName == authorDto.FirstName && a.LastName == authorDto.LastName);
                if (existingAuthor != null)
                {
                    _logger.Information("Author(s) already exists");
                    return authors;
                }
                else
                {
                    var author = _mapper.Map<Author>(authorDto);
                    var addedAuthor = await _authorRepository.AddAsync(author, cancellationToken);
                    authors.Add(addedAuthor);
                    _logger.Information("Author(s) were added");
                }
            }

            _logger.Information("The authors were returned");

            return authors;
        }


        private async Task<Media> CheckAndAddMedia(MediaToCreateDTO mediaDto, ICollection<Author> authors, CancellationToken cancellationToken)
        {
            var existingMedia = await _mediaRepository.GetByItemAsync(m => m.Name == mediaDto.Name && m.MediaType == mediaDto.MediaType);
            if (existingMedia != null)
            {
                _logger.Information("Media already exists");
                return existingMedia;
            }

            var media = _mapper.Map<Media>(mediaDto);
            media.Authors = authors;

            _logger.Information("Media has been returned");

            return await _mediaRepository.AddAsync(media, cancellationToken);
        }
    }
}

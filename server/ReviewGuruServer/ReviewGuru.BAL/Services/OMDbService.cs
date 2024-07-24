using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class OMDbSrvice(
        IReviewRepository reviewRepository,
        IMediaRepository mediaRepository,
        IAuthorRepository authorRepository,
        IMapper mapper,
        ILogger logger,
        IConfiguration configuration
        ) : IOMDbService
    {

        private readonly IReviewRepository _reviewRepository = reviewRepository;
        private readonly IMediaRepository _mediaRepository = mediaRepository;
        private readonly IAuthorRepository _authorRepository = authorRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public async Task<ReviewDTO> CreateWithAPIAsync(ReviewToCreateAPIDTO reviewAPIDto, int userId, CancellationToken cancellationToken = default)
        {
            var mediaDto = await GetMediaDataFromOmdbAsync(reviewAPIDto.MediaName, reviewAPIDto.YearOfMediaCreation);
            if (mediaDto == null)
            {
                _logger.Error($"Media data could not be retrieved for movie: {reviewAPIDto.MediaName}");
                throw new Exception($"Media data could not be retrieved for movie: {reviewAPIDto.YearOfMediaCreation}");
            }

            var review = await CreateAndAddReview(reviewAPIDto, userId, mediaDto, cancellationToken);

            _logger.Information("Review has been created");

            return _mapper.Map<ReviewDTO>(review);
        }

        private async Task<Review> CreateAndAddReview(ReviewToCreateAPIDTO reviewAPIDto, int userId, MediaToCreateDTO mediaDto, CancellationToken cancellationToken)
        {
            var media = await CheckAndAddMedia(mediaDto, cancellationToken);

            var existingReview = await _reviewRepository.GetByItemAsync(r => r.UserId == userId && r.MediaId == media.MediaId, cancellationToken);

            if (existingReview != null)
            {
                _logger.Error("You cannot create two reviews from the same user for the same entity");
                throw new BadRequestException("You cannot create two reviews from the same user for the same entity");
            }

            var review = _mapper.Map<Review>(reviewAPIDto);

            review.UserId = userId;
            review.Rating = reviewAPIDto.Rating;
            review.UserReview = reviewAPIDto.UserReview;
            review.MediaId = media.MediaId;
            review.DateOfCreation = DateTime.UtcNow;

            _logger.Information("Review has been added");

            return await _reviewRepository.AddAsync(review, cancellationToken);
        }

        private async Task<Media> CheckAndAddMedia(MediaToCreateDTO mediaDto, CancellationToken cancellationToken)
        {
            var existingMedia = await _mediaRepository.GetByItemAsync(m => m.Name.ToLower() == mediaDto.Name.ToLower() &&
                                                                      m.MediaType.ToLower() == mediaDto.MediaType.ToLower());
            
            if (existingMedia != null)
            {
                _logger.Information("Media already exists");
                return existingMedia;
            }

            var media = _mapper.Map<Media>(mediaDto);

            media.MediaType = mediaDto.MediaType[0].ToString().ToUpper() + mediaDto.MediaType[1..];

            var createdMedia = await _mediaRepository.AddAsync(media, cancellationToken);

            foreach (var authorDto in mediaDto.AuthorsToCreateDTO)
            {
                var existingAuthor = await _authorRepository.GetByItemAsync(a => a.FirstName == authorDto.FirstName && a.LastName == authorDto.LastName);

                if (existingAuthor == null)
                {
                    _logger.Information("Creating a new author");
                    var author = _mapper.Map<Author>(authorDto);
                    createdMedia.Authors.Add(author);
                    continue;
                }

                _logger.Information("Adding existing author");
                createdMedia.Authors.Add(existingAuthor);
            }

            createdMedia = await _mediaRepository.UpdateAsync(createdMedia, cancellationToken);

            _logger.Information("Media has been added");

            return createdMedia;
        }

        public async Task<MediaToCreateDTO> GetMediaDataFromOmdbAsync(string movieTitle, int? year = null)
        {
            dynamic movieData = await FetchMovieDataFromOmdbAsync(movieTitle, year);

            if (movieData == null)
            {
                _logger.Error($"Failed to get data from OMDb for movie: {movieTitle}");
                return null;
            }

            return CreateMediaDtoFromMovieData(movieData);
        }

        private async Task<dynamic> FetchMovieDataFromOmdbAsync(string movieTitle, int? year)
        {
            using HttpClient httpClient = new HttpClient();

            string apikey = _configuration["APISettings:APIKey"];

            string url = $"http://www.omdbapi.com/?t={movieTitle}" + (year != 0 ? $"&y={year}" : "") + $"&apikey={apikey}";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject(json);
            }
            else
            {
                return null;
            }
        }

        private MediaToCreateDTO CreateMediaDtoFromMovieData(dynamic movieData)
        {
            var authors = CreateAuthorDtoListFromMovieData(movieData);

            DateTime releaseDate;
            if (!DateTime.TryParseExact((string)movieData.Released, "dd MMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate))
            {
                _logger.Error($"Failed to parse release date for movie: {movieData.Title}");
                throw new Exception($"Failed to parse release date for movie: {movieData.Title}");
            }

            return new MediaToCreateDTO
            {
                MediaType = movieData.Type,
                Name = movieData.Title,
                YearOfCreating = DateOnly.FromDateTime(releaseDate),
                AuthorsToCreateDTO = authors
            };
        }

        private List<AuthorToCreateDTO> CreateAuthorDtoListFromMovieData(dynamic movieData)
        {
            var authors = new List<AuthorToCreateDTO>();

            if (movieData.Director != null)
            {
                authors.AddRange(CreateAuthorDtoListFromString(movieData.Director.ToString()));
            }

            return authors;
        }

        private List<AuthorToCreateDTO> CreateAuthorDtoListFromString(string authorsString)
        {
            var authors = new List<AuthorToCreateDTO>();

            string[] authorsArray = authorsString.Split(", ");
            foreach (string author in authorsArray)
            {
                string[] names = author.Split(" ");
                authors.Add(new AuthorToCreateDTO { FirstName = names[0], LastName = names[1] });
            }

            return authors;
        }

    }
}

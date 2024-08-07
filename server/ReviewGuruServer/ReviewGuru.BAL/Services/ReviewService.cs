﻿using AutoMapper;
using Microsoft.Extensions.Configuration.UserSecrets;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.BLL.Utilities.Exceptions;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System.Linq.Expressions;

namespace ReviewGuru.BLL.Services
{
    public class ReviewService(
        IReviewRepository reviewRepository,
        IMediaRepository mediaRepository,
        IAuthorRepository authorRepository,
        IMapper mapper,
        ILogger logger
        ) : IReviewService
    {
        private readonly IReviewRepository _reviewRepository = reviewRepository;

        private readonly IMediaRepository _mediaRepository = mediaRepository;

        private readonly IAuthorRepository _authorRepository = authorRepository;

        private readonly IMapper _mapper = mapper;

        private readonly ILogger _logger = logger;

        public async Task<IEnumerable<Review>> GetAllAsync(
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            int? minRating = null,
            int? maxRating = null,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Review, bool>> filter = (review) =>
                (review.DateOfDeleting == null) &&
                (string.IsNullOrEmpty(mediaType) || review.Media.MediaType == mediaType) &&
                (string.IsNullOrEmpty(searchText) ||
                 (!string.IsNullOrEmpty(review.UserReview) && review.UserReview.Contains(searchText)) ||
                 review.Media.Name.Contains(searchText) ||
                 review.Media.Authors.Any(author => (author.LastName + " " + author.FirstName).Contains(searchText))) &&
                (!minRating.HasValue || review.Rating >= minRating.Value) &&
                (!maxRating.HasValue || review.Rating <= maxRating.Value);

            _logger.Information("Page {0} of review has been returned", pageNumber);

            return await _reviewRepository.GetAllAsync(pageNumber, pageSize, filter, cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Review>> GetCurrentUserReviewsAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            int? minRating = null,
            int? maxRating = null,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Review, bool>> filter = (review) =>
                (review.DateOfDeleting == null) &&
                (review.UserId == userId) &&
                (string.IsNullOrEmpty(mediaType) || review.Media.MediaType == mediaType) &&
                (string.IsNullOrEmpty(searchText) ||
                 (!string.IsNullOrEmpty(review.UserReview) && review.UserReview.Contains(searchText)) ||
                 review.Media.Name.Contains(searchText) ||
                 review.Media.Authors.Any(author => (author.LastName + " " + author.FirstName).Contains(searchText))) &&
                (!minRating.HasValue || review.Rating >= minRating.Value) &&
                (!maxRating.HasValue || review.Rating <= maxRating.Value);

            _logger.Information("Page {0} of currrent user review has been returned", pageNumber);

            return await _reviewRepository.GetAllWithMediaAsync(pageNumber, pageSize, filter, cancellationToken: cancellationToken);
        }

        public async Task<int> GetCurrentUserReviewsCountAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            int? minRating = null,
            int? maxRating = null,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Review, bool>> filter = (review) =>
                (review.DateOfDeleting == null) &&
                (review.UserId == userId) &&
                (string.IsNullOrEmpty(mediaType) || review.Media.MediaType == mediaType) &&
                (string.IsNullOrEmpty(searchText) ||
                 (!string.IsNullOrEmpty(review.UserReview) && review.UserReview.Contains(searchText)) ||
                 review.Media.Name.Contains(searchText) ||
                 review.Media.Authors.Any(author => (author.LastName + " " + author.FirstName).Contains(searchText))) &&
                (!minRating.HasValue || review.Rating >= minRating.Value) &&
                (!maxRating.HasValue || review.Rating <= maxRating.Value);

            _logger.Information("Page {0} of currrent user review has been returned", pageNumber);

            return await _reviewRepository.CountAsync(filter, cancellationToken);
        }

        public async Task<IEnumerable<Review>> GetAllExceptCurrentUserReviewsAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            int? minRating = null,
            int? maxRating = null,
            CancellationToken cancellationToken = default)
        {
            Expression<Func<Review, bool>> filter = (review) =>
                (review.DateOfDeleting == null) &&
                (review.UserId != userId) &&
                (string.IsNullOrEmpty(mediaType) || review.Media.MediaType == mediaType) &&
                (string.IsNullOrEmpty(searchText) ||
                 (!string.IsNullOrEmpty(review.UserReview) && review.UserReview.Contains(searchText)) ||
                 review.Media.Name.Contains(searchText) ||
                 review.Media.Authors.Any(author => (author.LastName + " " + author.FirstName).Contains(searchText))) &&
                (!minRating.HasValue || review.Rating >= minRating.Value) &&
                (!maxRating.HasValue || review.Rating <= maxRating.Value);

            _logger.Information("Page {0} of review except current users reviews has been returned", pageNumber);

            return await _reviewRepository.GetAllAsync(pageNumber, pageSize, filter, cancellationToken: cancellationToken);
        }

        public async Task<ReviewDTO> CreateAsync(ReviewToCreateDTO reviewDto, int userId, CancellationToken cancellationToken = default)
        {
            var authors = await CheckAndAddAuthors(reviewDto.MediaToCreateDTO.AuthorsToCreateDTO, cancellationToken);

            var media = await CheckAndAddMedia(reviewDto.MediaToCreateDTO, authors, cancellationToken);

            var review = await CreateAndAddReview(reviewDto, userId, media, cancellationToken);

            _logger.Information("Review has been created");

            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<ReviewDTO> UpdateAsync(ReviewDTO dto, CancellationToken cancellationToken = default)
        {
            var entityToUpdate = _mapper.Map<Review>(dto);

            _logger.Information("Review has been updated");

            return _mapper.Map<ReviewDTO>(await _reviewRepository.UpdateAsync(entityToUpdate, cancellationToken: cancellationToken));
        }

        public async Task<ReviewDTO> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default)
        {
            var entityToDelete = await _reviewRepository.GetByItemAsync(i => i.ReviewId == id && i.UserId == userId && i.DateOfDeleting == null, cancellationToken);

            if (entityToDelete == null)
            {
                _logger.Error("Review with ID {0} is not found!", id);

                throw new NotFoundException("Review with this ID is not found!");
            }

            _logger.Information("Review has been removed");

            return _mapper.Map<ReviewDTO>(await _reviewRepository.DeleteAsync(entityToDelete.ReviewId, cancellationToken: cancellationToken));
        }

        private async Task<ICollection<Author>> CheckAndAddAuthors(ICollection<AuthorToCreateDTO> authorDtos, CancellationToken cancellationToken)
        {
            var authors = new List<Author>();

            foreach (var authorDto in authorDtos)
            {
                var existingAuthor = await _authorRepository.GetByItemAsync(a => a.FirstName == authorDto.FirstName && a.LastName == authorDto.LastName, cancellationToken);

                if (existingAuthor != null)
                {
                    _logger.Information("Author(s) already exists");
                    authors.Add(existingAuthor);
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
            var existingMedia = await _mediaRepository.GetByItemAsync(m => m.Name == mediaDto.Name && m.MediaType == mediaDto.MediaType, cancellationToken);

            if (existingMedia != null)
            {
                _logger.Information("Media already exists");
                return existingMedia;
            }

            var media = _mapper.Map<Media>(mediaDto);

            var createdMedia = await _mediaRepository.AddAsync(media, cancellationToken);

            foreach (var author in authors)
            {
                createdMedia.Authors.Add(author);
            }

            createdMedia = await _mediaRepository.UpdateAsync(createdMedia, cancellationToken);

            _logger.Information("Media has been returned");

            return createdMedia;
        }

        private async Task<Review> CreateAndAddReview(ReviewToCreateDTO reviewDto, int userId, Media media, CancellationToken cancellationToken)
        {
            var review = _mapper.Map<Review>(reviewDto);

            review.UserId = userId;
            review.MediaId = media.MediaId;
            review.DateOfCreation = DateTime.UtcNow;

            var existingReview = await _reviewRepository.GetByItemAsync(r => r.UserId == review.UserId && r.MediaId == review.MediaId, cancellationToken);

            if (existingReview != null)
            {
                _logger.Error("You cannot create two reviews from the same user for the same entity");

                throw new BadRequestException("You cannot create two reviews from the same user for the same entity");
            }

            _logger.Information("Review has been returned");

            return await _reviewRepository.AddAsync(review, cancellationToken);
        }
    }
}

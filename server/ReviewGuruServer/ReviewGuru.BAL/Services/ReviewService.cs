using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Services.IServices;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class ReviewService(
        IGenericRepository<Review> genericRepository, 
        IMapper mapper
        ) :  IReviewService
    {
        private readonly IGenericRepository<Review> _genericRepository = genericRepository;
        private readonly IMapper _mapper = mapper;
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
                (string.IsNullOrEmpty(mediaType) || review.Media.MediaType == mediaType) &&
                (string.IsNullOrEmpty(searchText) ||
                 review.UserReview.Contains(searchText) ||
                 review.User.Login.Contains(searchText) ||
                 review.Media.Name.Contains(searchText) ||
                 review.Media.Authors.Any(author => (author.LastName + " " + author.FirstName).Contains(searchText))) &&
                (!minRating.HasValue || review.Rating >= minRating.Value) &&
                (!maxRating.HasValue || review.Rating <= maxRating.Value);

            return await _genericRepository.GetAllAsync(pageNumber, pageSize, filter, cancellationToken: cancellationToken);
        }

        public async Task<ReviewDTO> CreateAsync(ReviewDTO reviewDto, CancellationToken cancellationToken = default)
        {
            var review = _mapper.Map<Review>(reviewDto);
            review.DateOfCreation = DateTime.UtcNow;

            var createdReview = await _genericRepository.AddAsync(review, cancellationToken);
            return _mapper.Map<ReviewDTO>(createdReview);
        }

        public async Task<ReviewDTO> UpdateAsync(ReviewDTO dto, CancellationToken cancellationToken = default)
        {
            var entityToUpdate = _mapper.Map<Review>(dto);

            return _mapper.Map<ReviewDTO>(await _genericRepository.UpdateAsync(entityToUpdate, cancellationToken: cancellationToken));
        }

        public async Task<ReviewDTO> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {

            var entityToDelete = _mapper.Map<Review>(id);


            return _mapper.Map<ReviewDTO>(await _genericRepository.DeleteAsync(entityToDelete.ReviewId, cancellationToken: cancellationToken));
        }
    }
}


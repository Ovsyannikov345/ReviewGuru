using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{
    public interface IReviewService
    {
        public Task<IEnumerable<Review>> GetAllAsync(
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            int? minRating = null,
            int? maxRating = null,
            CancellationToken cancellationToken = default);

        public Task<IEnumerable<Review>> GetCurrentUserReviewsAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            int? minRating = null,
            int? maxRating = null,
            CancellationToken cancellationToken = default);

        public Task<int> GetCurrentUserReviewsCountAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            int? minRating = null,
            int? maxRating = null,
            CancellationToken cancellationToken = default);

        public Task<IEnumerable<Review>> GetAllExceptCurrentUserReviewsAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            int? minRating = null,
            int? maxRating = null,
            CancellationToken cancellationToken = default);

        public Task<ReviewDTO> CreateAsync(ReviewToCreateDTO reviewDto, int userId, CancellationToken cancellationToken = default);


        public Task<ReviewDTO> UpdateAsync(ReviewDTO dto, CancellationToken cancellationToken = default);


        public Task<ReviewDTO> DeleteAsync(int id, int userId, CancellationToken cancellationToken = default);
    }
}

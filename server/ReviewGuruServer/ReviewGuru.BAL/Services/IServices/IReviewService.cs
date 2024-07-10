using ReviewGuru.BLL.DTOs;
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
        public Task<IEnumerable<ReviewDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);


        public Task<ReviewDTO> CreateAsync(ReviewDTO dto, CancellationToken cancellationToken = default);


        public Task<ReviewDTO> UpdateAsync(ReviewDTO dto, CancellationToken cancellationToken = default);


        public Task<ReviewDTO> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}

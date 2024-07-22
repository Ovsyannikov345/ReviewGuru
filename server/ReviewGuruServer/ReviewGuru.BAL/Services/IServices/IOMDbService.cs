using ReviewGuru.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{
    public interface IOMDbService
    {
        Task<ReviewDTO> CreateWithAPIAsync(ReviewToCreateAPIDTO reviewAPIDto, int userId, CancellationToken cancellationToken = default);
    }
}

using ReviewGuru.BLL.DTOs;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{
    public interface IMediaService 
    {
        Task<IEnumerable<MediaDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<MediaDTO> CreateAsync(MediaDTO dto, CancellationToken cancellationToken = default);
    }
}

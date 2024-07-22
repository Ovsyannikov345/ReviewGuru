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

    public interface IMediaService
    {
        public Task<IEnumerable<Media>> GetMediaListAsync(
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            CancellationToken cancellationToken = default);

        public Task<int> GetMediaCountAsync(string searchText = "", string mediaType = "", CancellationToken cancellationToken = default);

        public Task AddMediaToFavoritesAsync(int userId, int mediaId, CancellationToken cancellationToken = default);

        public Task RemoveMediaFromFavoritesAsync(int userId, int mediaId, CancellationToken cancellationToken = default);
    }
}

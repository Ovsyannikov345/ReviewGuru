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
    public interface IUserService
    {
        public Task<IEnumerable<Media>> GetUserFavoritesAsync(
            int userId,
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.PageSize,
            string searchText = "",
            string mediaType = "",
            CancellationToken cancellationToken = default);

        public Task<int> GetUserFavoritesCountAsync(
            int userId,
            string searchText = "",
            string mediaType = "",
            CancellationToken cancellationToken = default);
    }
}

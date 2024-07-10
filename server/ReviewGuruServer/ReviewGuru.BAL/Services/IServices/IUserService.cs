using ReviewGuru.BLL.DTOs;
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
        public Task<IEnumerable<Media>> GetUserFavoritesAsync(int userId, CancellationToken cancellationToken = default);
    }
}

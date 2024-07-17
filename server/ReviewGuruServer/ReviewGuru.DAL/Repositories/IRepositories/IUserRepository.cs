using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User?> GetUserWithFavoritesAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default);

        public Task<IEnumerable<Media>?> GetUserFavoritesAsync(
            int userId,
            int pageNumber,
            int pageSize,
            Expression<Func<Media, bool>>? mediaFilter = null,
            CancellationToken cancellationToken = default);

        public Task<int> GetUserFavoritesCountAsync(
            int userId,
            Expression<Func<Media, bool>>? mediaFilter = null,
            CancellationToken cancellationToken = default);
    }
}

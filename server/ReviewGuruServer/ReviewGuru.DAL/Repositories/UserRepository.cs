using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using System.Linq.Expressions;

namespace ReviewGuru.DAL.Repositories
{
    public class UserRepository(ReviewGuruDbContext context) : GenericRepository<User>(context), IUserRepository
    {
        private readonly ReviewGuruDbContext _context = context;

        public async Task<User?> GetUserWithFavoritesAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
        {
            return await _context.Users.Include(u => u.Favorites).ThenInclude(media => media.Authors).FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<IEnumerable<Media>?> GetUserFavoritesAsync(int userId, int pageNumber, int pageSize, Expression<Func<Media, bool>>? mediaFilter = null, CancellationToken cancellationToken = default)
        {
            if (!(await _context.Users.AnyAsync(u => u.UserId == userId, cancellationToken)))
            {
                return null;
            }

            var favorites = _context.Users.Include(u => u.Favorites)
                                          .ThenInclude(media => media.Authors)
                                          .Where(u => u.UserId == userId)
                                          .SelectMany(u => u.Favorites);

            if (mediaFilter != null)
            {
                favorites = favorites.Where(mediaFilter);
            }

            return await favorites.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<int> GetUserFavoritesCountAsync(
            int userId,
            Expression<Func<Media, bool>>? mediaFilter = null,
            CancellationToken cancellationToken = default)
        {
            var favorites = _context.Users.Where(u => u.UserId == userId)
                                          .Include(u => u.Favorites)
                                          .SelectMany(u => u.Favorites);

            if (mediaFilter != null)
            {
                favorites = favorites.Where(mediaFilter);
            }

            return await favorites.CountAsync(cancellationToken);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System.Linq.Expressions;

namespace ReviewGuru.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ReviewGuruDbContext _context;
        private readonly ILogger _logger;

        public UserRepository(ReviewGuruDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> GetUserWithFavoritesAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.Include(u => u.Favorites).ThenInclude(media => media.Authors).FirstOrDefaultAsync(filter, cancellationToken);
            _logger.Information($"GetUserWithFavoritesAsync called. User: {user}");
            return user;
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

            var result = await favorites.OrderBy(m => m.MediaId)
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync(cancellationToken);

            _logger.Information($"GetUserFavoritesAsync called. Favorites count: {result.Count}");

            return result;
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

            int count = await favorites.CountAsync(cancellationToken);
            _logger.Information($"GetUserFavoritesCountAsync called. Favorites count: {count}");

            return count;
        }
    }

}

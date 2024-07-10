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
            return await _context.Users.Include(u => u.Favorites).FirstOrDefaultAsync(filter, cancellationToken);
        }
    }
}

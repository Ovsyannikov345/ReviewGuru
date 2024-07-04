using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;

namespace ReviewGuru.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ReviewGuruDbContext _context;
        public UserRepository(ReviewGuruDbContext context) : base(context) 
        { 
            _context = context;
        }
    }
}

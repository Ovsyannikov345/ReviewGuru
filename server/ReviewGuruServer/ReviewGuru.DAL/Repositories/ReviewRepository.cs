using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ReviewGuruDbContext _context;

        public ReviewRepository(ReviewGuruDbContext context) : base(context) 
        {
            _context = context;
        }
    }
}

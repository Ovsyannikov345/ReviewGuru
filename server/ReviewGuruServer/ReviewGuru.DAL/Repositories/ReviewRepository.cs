using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ReviewGuruDbContext _context;

        private readonly ILogger _logger;

        public ReviewRepository(ReviewGuruDbContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Review>> GetAllWithMediaAsync(int pageNumber,
            int pageSize,
            Expression<Func<Review, bool>>? filter = null,
            CancellationToken cancellationToken = default)
        {
            var reviews = filter == null
                ? _context.Reviews.Include(r => r.Media).ThenInclude(m => m.Authors)
                : _context.Reviews.Include(r => r.Media).ThenInclude(m => m.Authors).Where(filter);

            var result = await reviews.OrderBy(r => r.ReviewId)
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync(cancellationToken);

            _logger.Information($"GetAllWithMediaAsync called. Entities count: {result.Count}");

            return result;
        }
    }
}

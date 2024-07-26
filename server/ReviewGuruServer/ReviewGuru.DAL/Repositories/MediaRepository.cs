using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories
{
    public class MediaRepository(ReviewGuruDbContext context, ILogger logger) : GenericRepository<Media>(context, logger), IMediaRepository
    {
        private readonly ReviewGuruDbContext _context = context;

        private readonly ILogger _logger = logger;

        public override async Task<IEnumerable<Media>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<Media, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            var media = filter == null ? _context.Media : _context.Media.Where(filter);

            media = media.Include(m => m.Authors);

            var result = await media.OrderBy(m => m.MediaId)
                                    .Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync(cancellationToken);

            _logger.Information("GetAllAsync called. Media count: {0}", result.Count);

            return result;
        }

        public async Task<Media?> GetMediaWithReviewsAsync(int mediaId, CancellationToken cancellationToken = default)
        {
            Media? media = await _context.Media.Include(m => m.Authors)
                                               .Include(m => m.Reviews)
                                               .FirstOrDefaultAsync(m => m.MediaId == mediaId, cancellationToken);

            _logger.Information("GetMediaWithReviewsAsync called. Media id: {MediaId}", mediaId);

            return media;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Entities.Models;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories
{
    public class MediaRepository(ReviewGuruDbContext context) : GenericRepository<Media>(context), IMediaRepository
    {
        private readonly ReviewGuruDbContext _context = context;

        public override async Task<List<Media>> GetListAsync(int pageNumber, int pageSize, Expression<Func<Media, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            var media = filter == null ? _context.Media : _context.Media.Where(filter);

            media = media.Include(m => m.Authors);

            return await media.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }
    }
}

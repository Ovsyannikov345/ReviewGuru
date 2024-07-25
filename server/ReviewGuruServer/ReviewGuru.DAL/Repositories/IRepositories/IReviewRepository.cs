using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories.IRepositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        public Task<IEnumerable<Review>> GetAllWithMediaAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<Review, bool>>? filter = null,
            CancellationToken cancellationToken = default);
    }
}

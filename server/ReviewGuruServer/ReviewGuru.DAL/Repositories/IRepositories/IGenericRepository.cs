using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories.IRepositories
{
    public interface IGenericRepository<T> where T : class, new()
    {
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);

        Task<List<T>> GetListAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
    }
}

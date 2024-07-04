using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories.IRepositories
{
    public interface IGenericRepository<TDTO> where TDTO : class, new()
    { 
        Task<TDTO?> GetAsync(Expression<Func<TDTO, bool>> filter, CancellationToken cancellationToken = default);

        Task<List<TDTO>> GetListAsync(int pageNumber, int pageSize, Expression<Func<TDTO, bool>>? filter = null, CancellationToken cancellationToken = default);

        Task<TDTO> AddAsync(TDTO entity, CancellationToken cancellationToken = default);

        Task<TDTO> UpdateAsync(TDTO entity, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync(TDTO entity, CancellationToken cancellationToken = default);
    }

}

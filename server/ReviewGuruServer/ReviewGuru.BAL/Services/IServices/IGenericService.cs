using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{
    public interface IGenericService<TDTO, TEntity> 
        where TDTO : class, new()
        where TEntity : class, new()
    {
        Task<List<TDTO>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<TDTO> CreateAsync(TDTO dto, CancellationToken cancellationToken = default);
        Task<TDTO> UpdateAsync(TDTO updateDto, CancellationToken cancellationToken = default); 
        Task<TDTO> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}

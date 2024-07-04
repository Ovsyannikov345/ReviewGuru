using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories
{
    public class GenericRepository<TDTO> : IGenericRepository<TDTO> where TDTO : class, new()
    {
        private readonly ReviewGuruDbContext _context;

        public GenericRepository(ReviewGuruDbContext context)
        {
            _context = context;
        }


        public async Task<TDTO?> GetAsync(Expression<Func<TDTO, bool>> filter, CancellationToken cancellationToken = default)

        {
            return await _context.Set<TDTO>().AsNoTracking().FirstOrDefaultAsync(filter);
        }


        public async Task<List<TDTO>> GetListAsync(int pageNumber, int pageSize, Expression<Func<TDTO, bool>>? filter = null, CancellationToken cancellationToken = default)

        {
            var entities = filter == null ? _context.Set<TDTO>() : _context.Set<TDTO>().Where(filter);

            return await entities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }


        public async Task<TDTO> AddAsync(TDTO entity, CancellationToken cancellationToken = default)

        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }


        public async Task<TDTO> UpdateAsync(TDTO entity, CancellationToken cancellationToken = default)

        {
            _context.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }


        public async Task<int> DeleteAsync(TDTO entity, CancellationToken cancellationToken = default)

        {
            _context.Remove(entity);

            return await _context.SaveChangesAsync();
        }
    }
}

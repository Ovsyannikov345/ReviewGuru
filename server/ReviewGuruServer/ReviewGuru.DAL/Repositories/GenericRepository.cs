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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        private readonly ReviewGuruDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(ReviewGuruDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByItemAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)

        {
            return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            var entities = filter == null ? _dbSet : _dbSet.Where(filter);

            return await entities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<TEntity> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {

            var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            return entity;
        }
    }
}
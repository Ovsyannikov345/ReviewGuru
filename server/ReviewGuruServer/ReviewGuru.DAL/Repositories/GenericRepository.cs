using Microsoft.EntityFrameworkCore;
using ReviewGuru.DAL.Data;
using ReviewGuru.DAL.Repositories.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        private readonly ReviewGuruDbContext _context;

        protected readonly DbSet<TEntity> _dbSet;

        private readonly ILogger _logger;

        public GenericRepository(ReviewGuruDbContext context, ILogger logger)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _logger = logger;
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            int count = filter == null ? await _dbSet.CountAsync(cancellationToken) : await _dbSet.CountAsync(filter, cancellationToken);

            _logger.Information("CountAsync called. Count: {0}", count);

            return count;
        }

        public async Task<TEntity?> GetByItemAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(filter, cancellationToken);

            _logger.Information("GetByItemAsync called. Entity: {0}", entity);

            return entity;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
        {
            var entities = filter == null ? _dbSet : _dbSet.Where(filter);

            var result = await entities.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            _logger.Information("GetAllAsync called. Entities count: {0}", result.Count);

            return result;
        }

        public async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.Information("AddAsync called. Entity: {0}", entity);

            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.Information("UpdateAsync called. Entity: {0}", entity);

            return entity;
        }

        public async Task<TEntity?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync([id], cancellationToken: cancellationToken);

            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.Information("DeleteAsync called. Entity: {0}", entity);
            }

            return entity;
        }
    }
}

// ===============================
// GenericRepository
// ===============================

using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Common;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaridikExpress.Infrastructure.Repositories
{
    public class GenericRepository<TEntity>
        : IGenericRepository<TEntity>
        where TEntity : class
    {
        protected readonly ApplicationDbContext _dbContext;

        protected readonly DbSet<TEntity> _dbSet;



        public GenericRepository(ApplicationDbContext context)
        {
            _dbContext = context;

            _dbSet = context.Set<TEntity>();
        }



        // ==================== Query ====================

        public IQueryable<TEntity> Query()
            => _dbSet.AsNoTracking();



        public async Task<TEntity?> GetByIdAsync(
            Guid id,
            CancellationToken ct = default)
            => await _dbSet.FindAsync([id], ct);



        public async Task<TEntity?> GetByIntIdAsync(
            int id,
            CancellationToken ct = default)
            => await _dbSet.FindAsync([id], ct);



        public async Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, ct);



        public async Task<IReadOnlyList<TEntity>> GetAllAsync(
            CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .ToListAsync(ct);



        public async Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
            => await _dbSet
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(ct);



        public async Task<PagedResult<TEntity>> GetPagedAsync(
            int page,
            int pageSize,
            CancellationToken ct = default)
            => await GetPagedAsync(
                _dbSet.AsNoTracking(),
                page,
                pageSize,
                ct);



        public async Task<PagedResult<TEntity>> GetPagedAsync(
            IQueryable<TEntity> query,
            int page,
            int pageSize,
            CancellationToken ct = default)
        {
            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<TEntity>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }



        // ==================== Existence ====================

        public async Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync([id], ct);

            return entity != null;
        }



        public async Task<bool> ExistsAsync(
            Guid id,
            CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync([id], ct);

            return entity != null;
        }



        public async Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
            => await _dbSet.AnyAsync(predicate, ct);



        public async Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default)
            => await _dbSet.AnyAsync(predicate, ct);



        public async Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken ct = default)
            => predicate is null
                ? await _dbSet.CountAsync(ct)
                : await _dbSet.CountAsync(predicate, ct);



        // ==================== Commands ====================

        public async Task AddAsync(
            TEntity entity,
            CancellationToken ct = default)
        {
            await _dbSet.AddAsync(entity, ct);

            await _dbContext.SaveChangesAsync(ct);
        }



        public async Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken ct = default)
        {
            await _dbSet.AddRangeAsync(entities, ct);

            await _dbContext.SaveChangesAsync(ct);
        }



        public async Task<bool> UpdateAsync(
            TEntity entity,
            CancellationToken ct = default)
        {
            _dbSet.Update(entity);

            var affectedRows =
                await _dbContext.SaveChangesAsync(ct);

            return affectedRows > 0;
        }



        public void Update(TEntity entity)
            => _dbSet.Update(entity);



        public void UpdateRange(IEnumerable<TEntity> entities)
            => _dbSet.UpdateRange(entities);



        public async Task DeleteAsync(
            TEntity entity,
            CancellationToken ct = default)
        {
            _dbSet.Remove(entity);

            await _dbContext.SaveChangesAsync(ct);
        }



        public void Delete(TEntity entity)
            => _dbSet.Remove(entity);



        public async Task DeleteRangeAsync(
            ICollection<TEntity> entities,
            CancellationToken ct = default)
        {
            _dbSet.RemoveRange(entities);

            await _dbContext.SaveChangesAsync(ct);
        }



        public void DeleteRange(IEnumerable<TEntity> entities)
            => _dbSet.RemoveRange(entities);
    }
}
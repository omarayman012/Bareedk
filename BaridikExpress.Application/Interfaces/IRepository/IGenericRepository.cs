// ===============================
// IGenericRepository
// ===============================

using BaridikExpress.Domain.Common;
using System.Linq.Expressions;

namespace BaridikExpress.Application.Interfaces.IRepository
{
    public interface IGenericRepository<TEntity>
        where TEntity : class
    {
        // ==================== Query ====================

        IQueryable<TEntity> Query();

        Task<TEntity?> GetByIdAsync(
            Guid id,
            CancellationToken ct = default);

        Task<TEntity?> GetByIntIdAsync(
            int id,
            CancellationToken ct = default);

        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);

        Task<IReadOnlyList<TEntity>> GetAllAsync(
            CancellationToken ct = default);

        Task<IReadOnlyList<TEntity>> FindAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);

        Task<PagedResult<TEntity>> GetPagedAsync(
            int page,
            int pageSize,
            CancellationToken ct = default);

        Task<PagedResult<TEntity>> GetPagedAsync(
            IQueryable<TEntity> query,
            int page,
            int pageSize,
            CancellationToken ct = default);



        // ==================== Existence ====================

        Task<bool> ExistsAsync(
            int id,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            Guid id,
            CancellationToken ct = default);

        Task<bool> ExistsAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);

        Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);

        Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken ct = default);



        // ==================== Commands ====================

        Task AddAsync(
            TEntity entity,
            CancellationToken ct = default);

        Task AddRangeAsync(
            IEnumerable<TEntity> entities,
            CancellationToken ct = default);

        Task<bool> UpdateAsync(
            TEntity entity,
            CancellationToken ct = default);

        void Update(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        Task DeleteAsync(
            TEntity entity,
            CancellationToken ct = default);

        void Delete(TEntity entity);

        Task DeleteRangeAsync(
            ICollection<TEntity> entities,
            CancellationToken ct = default);

        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
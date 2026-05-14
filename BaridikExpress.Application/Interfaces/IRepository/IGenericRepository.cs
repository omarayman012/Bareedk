using BaridikExpress.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        // ==================== Query ====================
        IQueryable<TEntity> Query();

        Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default);

        Task<TEntity?> FirstOrDefaultAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);

        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default);

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
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken ct = default);

        Task<int> CountAsync(
            Expression<Func<TEntity, bool>>? predicate = null,
            CancellationToken ct = default);

        // ==================== Commands ====================
        Task AddAsync(TEntity entity, CancellationToken ct = default);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
    }
    }
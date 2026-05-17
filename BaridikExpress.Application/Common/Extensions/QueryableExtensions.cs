using BaridikExpress.Application.Common.Models;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Application.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyCommonFilters<T>(
       this IQueryable<T> query,
       BaseFilter filter)
       where T : BaseEntity
        {
            if (filter.IsActive.HasValue)
            {
                query = query.Where(x =>
                    x.IsActive == filter.IsActive);
            }

            if (!string.IsNullOrWhiteSpace(filter.CreatedById))
            {
                query = query.Where(x =>
                    x.CreatedById == filter.CreatedById);
            }

            if (filter.FromDate.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt >= filter.FromDate);
            }

            if (filter.ToDate.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt <= filter.ToDate);
            }

            return query;
        }
    }
}

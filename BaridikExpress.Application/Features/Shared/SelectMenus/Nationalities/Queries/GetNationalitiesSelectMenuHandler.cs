using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.Shared.SelectMenus.Nationalities.DTOs;

namespace BaridikExpress.Application.Features.Shared.SelectMenus.Nationalities.Queries
{
    public sealed class GetNationalitiesSelectMenuHandler
        : IRequestHandler<
            GetNationalitiesSelectMenuQuery,
            IReadOnlyList<SelectMenuDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetNationalitiesSelectMenuHandler(
            IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<SelectMenuDto>> Handle(
            GetNationalitiesSelectMenuQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<Domain.Entities.Nationality.Nationality> query =
                _context.Nationalities
                    .AsNoTracking()
                    .Where(x => x.IsActive);

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var search = request.Name.Trim();

                query = query.Where(x =>
                    EF.Functions.Like(x.Name, $"%{search}%"));
            }

            return await query
                .OrderBy(x => x.Name)
                .Select(x => new SelectMenuDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(cancellationToken);
        }
    }
}

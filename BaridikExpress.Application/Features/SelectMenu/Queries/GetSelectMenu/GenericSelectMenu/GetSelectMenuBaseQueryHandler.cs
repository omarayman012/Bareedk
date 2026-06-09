using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.GenericSelectMenu
{
    public class GetSelectMenuBaseQueryHandler<T>(
        IApplicationDbContext db,
        IStringLocalizer localizer)
        : IRequestHandler<GetSelectMenubaseQuery<T>, Result<IEnumerable<SelectMenuResponse>>>
        where T : BaseEntity, ISelectMenuEntity
    {
        public async Task<Result<IEnumerable<SelectMenuResponse>>> Handle(
            GetSelectMenubaseQuery<T> request,
            CancellationToken cancellationToken)
        {
            var entityName = typeof(T).Name;

            var query = db.Set<T>().AsNoTracking();

            if (request.ParentId.HasValue)
                query = query.Where(x => x.ParentId == request.ParentId);

            if (!string.IsNullOrWhiteSpace(request.Name))
                query = query.Where(x =>
                    x.NameAr!.Contains(request.Name) ||
                    x.NameEn!.Contains(request.Name));

            var result = await query
                .Select(x => new SelectMenuResponse(
                    x.Id,
                    new LocalizeLang
                    {
                        AR = x.NameAr ?? string.Empty,
                        EN = x.NameEn ?? string.Empty
                    }))
                .ToListAsync(cancellationToken);

            if (!result.Any())
                return Result<IEnumerable<SelectMenuResponse>>
                    .Failure(localizer[$"{entityName}NotFound"], 404);

            return Result<IEnumerable<SelectMenuResponse>>
                .Success(result, localizer[$"{entityName}RetrievedSuccessfully"]);
        }
    }

}

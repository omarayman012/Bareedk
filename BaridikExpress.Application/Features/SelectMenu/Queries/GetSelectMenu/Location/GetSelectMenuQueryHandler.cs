using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Location;

public sealed class GetSelectMenuQueryHandler<T>(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetSelectMenuQuery<T>, Result<IEnumerable<SelectMenuResponse>>>
       where T : BaseEntity, ISelectMenuEntity
{
    #region Handle
    public async Task<Result<IEnumerable<SelectMenuResponse>>> Handle(
        GetSelectMenuQuery<T> request,
        CancellationToken cancellationToken)
    {
        var entityName = typeof(T).Name;

        #region Fetch Entities
        var allEntities = await db.Set<T>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var entities = allEntities
            .Where(x => !request.ParentId.HasValue || x.ParentId == request.ParentId)
            .Where(x => string.IsNullOrEmpty(request.Name) ||
                        (x.NameAr != null && x.NameAr.Contains(request.Name, StringComparison.OrdinalIgnoreCase)) ||
                        (x.NameEn != null && x.NameEn.Contains(request.Name, StringComparison.OrdinalIgnoreCase)))
            .ToList();

        if (!entities.Any())
            return Result<IEnumerable<SelectMenuResponse>>
                .Failure(localizer[$"{entityName}NotFound"], 404);
        #endregion

        #region Map Response
        var response = entities.Select(x => new SelectMenuResponse(
            x.Id,
            new LocalizeLang
            {
                AR = x.NameAr ?? string.Empty,
                EN = x.NameEn ?? string.Empty
            }));
        #endregion

        return Result<IEnumerable<SelectMenuResponse>>
            .Success(response, localizer[$"{entityName}RetrievedSuccessfully"]);
    }
    #endregion
}
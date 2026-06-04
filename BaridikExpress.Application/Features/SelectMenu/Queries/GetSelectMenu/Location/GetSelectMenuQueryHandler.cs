using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Domain.Common;
using BaridikExpress.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Location;

public sealed class GetSelectMenuQueryHandler<T>(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetSelectMenuQuery<T>, Result<IEnumerable<SelectMenuResponse>>>
       where T : BaseEntity, ISelectMenuEntity, new()
{
    #region Handle
    public async Task<Result<IEnumerable<SelectMenuResponse>>> Handle(
        GetSelectMenuQuery<T> request,
        CancellationToken cancellationToken)
    {
        var entityName = typeof(T).Name;

        #region Fetch Entities
        var entities = await db.Set<T>()
         .AsNoTracking()
         .Where(x => request.ParentId == null || x.ParentId == request.ParentId)
         .Where(x => request.Name == null ||
                     x.NameAr!.Contains(request.Name) ||
                     x.NameEn!.Contains(request.Name)) 
         .ToListAsync(cancellationToken);

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
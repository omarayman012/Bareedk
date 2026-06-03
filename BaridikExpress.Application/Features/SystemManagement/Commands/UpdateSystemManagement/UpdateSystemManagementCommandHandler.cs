using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SystemManagement.DTOs;
using BaridikExpress.Domain.Entities.SystemManagment;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSystemManagement;

public sealed class UpdateSystemManagementCommandHandler<T>(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateSystemManagementCommand<T>, Result<SystemManagementResponse>>
    where T : BaseSystemManagementEntity, new()
{
    #region Handle
    public async Task<Result<SystemManagementResponse>> Handle(
        UpdateSystemManagementCommand<T> request,
        CancellationToken cancellationToken)
    {
        var entityName = typeof(T).Name;

        #region Fetch Entity
        var entity = await db.Set<T>()
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            return Result<SystemManagementResponse>
                .Failure(localizer[$"{entityName}NotFound"], 404);
        #endregion

        #region Update & Save
        entity.Update(request.DescriptionAr, request.DescriptionEn);
        await db.SaveChangesAsync(cancellationToken);
        #endregion

        #region Reload UpdatedBy
        await db.Entry(entity)
            .Reference(x => x.UpdatedBy)
            .LoadAsync(cancellationToken);
        #endregion

        #region Map Response
        var response = new SystemManagementResponse(
            entity.Id,
            new LocalizeLang
            {
                AR = entity.DescriptionAr ?? string.Empty,
                EN = entity.DescriptionEn ?? string.Empty
            },
            entity.UpdatedBy?.FullName,
            entity.UpdatedAt);
        #endregion

        return Result<SystemManagementResponse>
            .Success(response, localizer[$"{entityName}UpdatedSuccessfully"]);
    }
    #endregion
}
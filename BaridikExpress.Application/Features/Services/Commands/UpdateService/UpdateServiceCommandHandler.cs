using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Services.Commands.UpdateService;

public sealed class UpdateServiceCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<UpdateServiceCommand, Result<bool>>
{
    #region Handle
    public async Task<Result<bool>> Handle(
        UpdateServiceCommand request,
        CancellationToken cancellationToken)
    {
        #region Fetch Service
        var service = await db.Services
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (service is null)
            return Result<bool>.Failure(localizer["ServiceNotFound"], 404);
        #endregion

        #region Trim (if sent)
        var nameEn = request.NameEn?.Trim();
        var nameAr = request.NameAr?.Trim();
        #endregion

        #region Validate Uniqueness (if name sent)
        if (nameEn is not null || nameAr is not null)
        {
            var nameExists = await db.Services
                .AnyAsync(x =>
                    x.Id != request.Id &&
                    (x.NameEn == nameEn || x.NameAr == nameAr),
                    cancellationToken);
            if (nameExists)
                return Result<bool>.Failure(localizer["ServiceAlreadyExists"]);
        }
        #endregion

        #region Upload Image (if sent)
        var imageUrl = service.ImageUrl;
        if (request.Image is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";
            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "service-images");
            if (imageUrl is null)
                return Result<bool>.Failure(localizer["ImageUploadFailed"], 400);
        }
        #endregion

        #region Update & Save
        service.Update(nameEn, nameAr, request.Price, request.Currency, imageUrl);
        await db.SaveChangesAsync(cancellationToken);
        #endregion

        return Result<bool>.Success(true, localizer["ServiceUpdatedSuccessfully"]);
    }
    #endregion
}
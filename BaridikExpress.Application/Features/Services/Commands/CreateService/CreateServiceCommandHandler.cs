using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.Services.DTOs;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Services;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Services.Commands.CreateService;

public sealed class CreateServiceCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer,
    IFileStorageService fileStorage)
    : IRequestHandler<CreateServiceCommand, Result<ServiceResponse>>
{
    #region Handle

    public async Task<Result<ServiceResponse>> Handle(
        CreateServiceCommand request,
        CancellationToken cancellationToken)
    {
        #region Normalize Name

        var (nameAr, nameEn) = NormalizeHelper.Normalize(request.NameAr, request.NameEn);

        #endregion

        #region Validate Uniqueness

        var nameExists = await db.Services
            .AnyAsync(x => x.NameEn == nameEn || x.NameAr == nameAr, cancellationToken);

        if (nameExists)
            return Result<ServiceResponse>.Failure(localizer["ServiceAlreadyExists"]);

        #endregion

        #region Upload Image

        string? imageUrl = null;

        if (request.Image is not null)
        {
            var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(request.Image.FileName)}";

            imageUrl = await fileStorage.SaveFileAsync(
                request.Image.OpenReadStream(),
                uniqueFileName,
                "service-images");

            if (imageUrl is null)
                return Result<ServiceResponse>.Failure(localizer["ImageUploadFailed"], 400);
        }

        #endregion

        #region Create & Save

        var service = Service.Create(nameEn, nameAr, request.Price, imageUrl);
        await db.Services.AddAsync(service, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        #endregion

        #region Map Response

        var response = new ServiceResponse(
            service.Id,
            new LocalizedDto { EN = service.NameEn, AR = service.NameAr },
            service.Price,
            service.ImageUrl,
            service.IsActive,
            service.CreatedBy?.FullName,
            service.CreatedAt,
            service.UpdatedBy?.FullName,
            service.UpdatedAt);

        #endregion

        return Result<ServiceResponse>.Success(response, localizer["ServiceCreatedSuccessfully"], 201);
    }

    #endregion
}

using Awael_Al_Joudah.Application.DTO.BlogsCategoryModules;
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.BlogsModules;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules;

public class CreateBlogsCategoryHandler(
    IApplicationDbContext context,
    UserManager<User> userManager,
    IGetCurrentUserRepository getCurrentUser,
    IFileStorageService fileStorageService,
    IStringLocalizer localizer,
    ILogger<CreateBlogsCategoryHandler> logger)
            : IRequestHandler<CreateBlogsCategoryCommand, Result<ResponseBlogsCategoryDto>>
{
    public async Task<Result<ResponseBlogsCategoryDto>> Handle(
        CreateBlogsCategoryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = getCurrentUser.GetUserId();
            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return Result<ResponseBlogsCategoryDto>.Failure(
                    localizer["Unauthorized"], 401);
            }

            var currentUser = await userManager.FindByIdAsync(currentUserId);
            if (currentUser is null)
            {
                return Result<ResponseBlogsCategoryDto>.Failure(
                    localizer["UserNotFound"], 404);
            }

            var normalizedData = NormalizeLocalizedFields(
                request.NameAr,
                request.NameEn,
                request.DescriptionAr,
                request.DescriptionEn);

            var isDuplicate = await context.BlogsCategorys.AnyAsync(
                x => x.NameAr.ToLower() == normalizedData.NameAr.ToLower()
                  || x.NameEn.ToLower() == normalizedData.NameEn.ToLower(),
                cancellationToken);

            if (isDuplicate)
            {
                return Result<ResponseBlogsCategoryDto>.Failure(
                    localizer["BlogsCategoryAlreadyExists"], 400);
            }

            var imageUrl = await SaveImageAsync(request.Image, cancellationToken);

            var category = new BlogsCategory
            {
                Id = Guid.NewGuid(),
                NameAr = normalizedData.NameAr,
                NameEn = normalizedData.NameEn,
                DescriptionAr = normalizedData.DescriptionAr,
                DescriptionEn = normalizedData.DescriptionEn,
                Priorty = request.Priorty,
                Image = imageUrl,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedById = currentUserId
            };

            context.BlogsCategorys.Add(category);
            await context.SaveChangesAsync(cancellationToken);

            var response = MapToResponse(category, currentUser.FullName);

            return Result<ResponseBlogsCategoryDto>.Success(
                response,
                localizer["BlogsCategoryCreatedSuccessfully"],
                201);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while creating blog category");
            return Result<ResponseBlogsCategoryDto>.Error(
                localizer["UnexpectedError"],
                500);
        }
    }

    private static (string NameAr, string NameEn, string? DescriptionAr, string? DescriptionEn)
        NormalizeLocalizedFields(
            string? nameAr,
            string? nameEn,
            string? descriptionAr,
            string? descriptionEn)
    {
        var nameArInput = nameAr?.Trim();
        var nameEnInput = nameEn?.Trim();

        var finalNameAr = string.IsNullOrWhiteSpace(nameArInput) ? nameEnInput! : nameArInput;
        var finalNameEn = string.IsNullOrWhiteSpace(nameEnInput) ? nameArInput! : nameEnInput;

        var descArInput = descriptionAr?.Trim();
        var descEnInput = descriptionEn?.Trim();

        string? finalDescAr = null;
        string? finalDescEn = null;

        if (!string.IsNullOrWhiteSpace(descArInput) || !string.IsNullOrWhiteSpace(descEnInput))
        {
            finalDescAr = string.IsNullOrWhiteSpace(descArInput) ? descEnInput : descArInput;
            finalDescEn = string.IsNullOrWhiteSpace(descEnInput) ? descArInput : descEnInput;
        }

        return (finalNameAr, finalNameEn, finalDescAr, finalDescEn);
    }

    private async Task<string> SaveImageAsync(
        Microsoft.AspNetCore.Http.IFormFile image,
        CancellationToken cancellationToken)
    {
        if (image == null || image.Length == 0)
            return string.Empty;

        using var stream = image.OpenReadStream();

        return await fileStorageService.SaveFileAsync(
            stream,
            image.FileName,
            "BlogsCategories") ?? string.Empty;
    }

    private static ResponseBlogsCategoryDto MapToResponse(
        BlogsCategory category,
        string createdBy)
    {
        return new ResponseBlogsCategoryDto
        {
            Id = category.Id,
            Name = new LocalizedDto
            {
                AR = category.NameAr,
                EN = category.NameEn
            },
            Priorty = category.Priorty,
            Description = new LocalizedDto
            {
                AR = category.DescriptionAr,
                EN = category.DescriptionEn
            },
            Image = category.Image,
            IsActive = category.IsActive,
            CreatedBy = createdBy
        };
    }
}
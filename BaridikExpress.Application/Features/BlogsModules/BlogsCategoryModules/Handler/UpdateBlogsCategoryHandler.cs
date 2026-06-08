
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands;
using BaridikExpress.Application.Interfaces.File;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules
{
    public class UpdateBlogsCategoryHandler
        : IRequestHandler<UpdateBlogsCategoryCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IGetCurrentUserRepository _getCurrentUserRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IStringLocalizer _localizer;
        private readonly UserManager<User> _userManager;

        public UpdateBlogsCategoryHandler(
            IApplicationDbContext applicationDbContext,
            IGetCurrentUserRepository getCurrentUserRepository,
            IFileStorageService fileStorageService,
            IStringLocalizer localizer,
            UserManager<User> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _getCurrentUserRepository = getCurrentUserRepository;
            _fileStorageService = fileStorageService;
            _localizer = localizer;
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(
            UpdateBlogsCategoryCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _getCurrentUserRepository.GetUserId();

                if (string.IsNullOrWhiteSpace(currentUserId))
                {
                    return Result<bool>.Failure(_localizer["Unauthorized"], 401);
                }

                var currentUser = await _userManager.FindByIdAsync(currentUserId);

                if (currentUser == null)
                {
                    return Result<bool>.Failure(_localizer["UserNotFound"], 404);
                }

                var existingBlogsCategory = await _applicationDbContext.BlogsCategorys
                    .FirstOrDefaultAsync(category => category.Id == request.Id, cancellationToken);

                if (existingBlogsCategory == null)
                {
                    return Result<bool>.Failure(_localizer["BlogsCategoryNotFound"], 404);
                }

                if (request.NameAr != null)
                {
                    existingBlogsCategory.NameAr = request.NameAr.Trim();
                }

                if (request.NameEn != null)
                {
                    existingBlogsCategory.NameEn = request.NameEn.Trim();
                }

                if (string.IsNullOrWhiteSpace(existingBlogsCategory.NameAr) &&
                    string.IsNullOrWhiteSpace(existingBlogsCategory.NameEn))
                {
                    return Result<bool>.Failure(_localizer["NameArOrNameEnRequired"], 400);
                }

                var normalizedNameAr = existingBlogsCategory.NameAr?.Trim().ToLower();
                var normalizedNameEn = existingBlogsCategory.NameEn?.Trim().ToLower();

                var blogsCategoryAlreadyExists = await _applicationDbContext.BlogsCategorys
                    .AnyAsync(category =>
                        category.Id != request.Id &&
                        (
                            (!string.IsNullOrWhiteSpace(normalizedNameAr) &&
                             category.NameAr != null &&
                             category.NameAr.ToLower() == normalizedNameAr) ||
                            (!string.IsNullOrWhiteSpace(normalizedNameEn) &&
                             category.NameEn != null &&
                             category.NameEn.ToLower() == normalizedNameEn)
                        ),
                        cancellationToken);

                if (blogsCategoryAlreadyExists)
                {
                    return Result<bool>.Failure(_localizer["BlogsCategoryAlreadyExists"], 400);
                }

                if (request.DescriptionAr != null)
                {
                    existingBlogsCategory.DescriptionAr = string.IsNullOrWhiteSpace(request.DescriptionAr)
                        ? null
                        : request.DescriptionAr.Trim();
                }

                if (request.DescriptionEn != null)
                {
                    existingBlogsCategory.DescriptionEn = string.IsNullOrWhiteSpace(request.DescriptionEn)
                        ? null
                        : request.DescriptionEn.Trim();
                }

                if (request.Priorty.HasValue)
                {
                    existingBlogsCategory.Priorty = request.Priorty.Value;
                }

                if (request.IsActive.HasValue)
                {
                    existingBlogsCategory.IsActive = request.IsActive.Value;
                }

                if (request.Image != null && request.Image.Length > 0)
                {
                    using var imageStream = request.Image.OpenReadStream();

                    var uploadedImagePath = await _fileStorageService.SaveFileAsync(
                        imageStream,
                        request.Image.FileName,
                        "BlogsCategories");

                    if (!string.IsNullOrWhiteSpace(uploadedImagePath))
                    {
                        existingBlogsCategory.Image = uploadedImagePath;
                    }
                }

                existingBlogsCategory.UpdatedAt = DateTime.UtcNow;
                existingBlogsCategory.UpdatedById = currentUserId;

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(
                    true,
                    _localizer["BlogsCategoryUpdatedSuccessfully"],
                    200);
            }
            catch (Exception)
            {
                return Result<bool>.Error(
                    _localizer["UnexpectedError"],
                    500);
            }
        }
    }
}

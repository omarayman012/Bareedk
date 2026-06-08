
using BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Handler
{
    public class UpdateBlogsAuthorHandler : IRequestHandler<UpdateBlogsAuthorCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly IGetCurrentUserRepository _getCurrentUser;
        private readonly UserManager<User> _userManager;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<UpdateBlogsAuthorHandler> _logger;

        public UpdateBlogsAuthorHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            IGetCurrentUserRepository getCurrentUser,
            UserManager<User> userManager,
            IFileStorageService fileStorageService,
            ILogger<UpdateBlogsAuthorHandler> logger)
        {
            _context = context;
            _localizer = localizer;
            _getCurrentUser = getCurrentUser;
            _userManager = userManager;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(
            UpdateBlogsAuthorCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _getCurrentUser.GetUserId();

                if (string.IsNullOrWhiteSpace(currentUserId))
                    return Result<bool>.Failure(_localizer["Unauthorized"], 401);

                var currentUser = await _userManager.FindByIdAsync(currentUserId);

                if (currentUser is null)
                    return Result<bool>.Failure(_localizer["UserNotFound"], 404);

                var author = await _context.BlogsAuthors
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (author is null)
                    return Result<bool>.Failure(_localizer["BlogsAuthorNotFound"], 404);

                if (!string.IsNullOrWhiteSpace(request.NameAr))
                    author.NameAr = request.NameAr.Trim();

                if (!string.IsNullOrWhiteSpace(request.NameEn))
                    author.NameEn = request.NameEn.Trim();

                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    var normalizedEmail = request.Email.Trim().ToLowerInvariant();

                    var emailExists = await IsEmailTakenAsync(request.Id, normalizedEmail, cancellationToken);

                    if (emailExists)
                        return Result<bool>.Failure(_localizer["EmailAlreadyExists"], 400);

                    author.Email = normalizedEmail;
                }

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    var phoneNumber = request.PhoneNumber.Trim();

                    var phoneExists = await IsPhoneTakenAsync(request.Id, phoneNumber, cancellationToken);

                    if (phoneExists)
                        return Result<bool>.Failure(_localizer["PhoneNumberAlreadyExists"], 400);

                    author.PhoneNumber = phoneNumber;
                }

                if (HasValue(author.NameAr) || HasValue(author.NameEn))
                {
                    var nameExists = await IsNameTakenAsync(
                        request.Id,
                        author.NameAr,
                        author.NameEn,
                        cancellationToken);

                    if (nameExists)
                        return Result<bool>.Failure(_localizer["AuthorNameAlreadyExists"], 400);
                }
                if (request.Gender.HasValue)
                {
                    if (!Enum.IsDefined(typeof(UserGender), request.Gender.Value))
                        return Result<bool>.Failure(_localizer["InvalidGender"], 400);

                    author.Gender = request.Gender.Value;
                }
                if (request.BlogsCategoryId.HasValue)
                {
                    var category = await _context.BlogsCategorys
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == request.BlogsCategoryId.Value, cancellationToken);

                    if (category is null)
                        return Result<bool>.Failure(_localizer["BlogsCategoryNotFound"], 404);

                    author.BlogsCategoryId = request.BlogsCategoryId.Value;
                }

                if (request.IsActive.HasValue)
                    author.IsActive = request.IsActive.Value;

                if (request.Image is not null && request.Image.Length > 0)
                {
                    await using var stream = request.Image.OpenReadStream();

                    var updatedImageUrl = await _fileStorageService.UpdateFileAsync(
                        stream,
                        request.Image.FileName,
                        author.Image,
                        "BlogsAuthors");

                    if (string.IsNullOrWhiteSpace(updatedImageUrl))
                        return Result<bool>.Failure(_localizer["ImageUploadFailed"], 500);

                    author.Image = updatedImageUrl;
                }

                author.UpdatedAt = DateTime.UtcNow;
                author.UpdatedById = currentUserId;

                await _context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(true, _localizer["BlogsAuthorUpdatedSuccessfully"], 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating blog author with id {AuthorId}", request.Id);
                return Result<bool>.Error(_localizer["UnexpectedError"], 500);
            }
        }

        private async Task<bool> IsNameTakenAsync(
            Guid authorId,
            string? nameAr,
            string? nameEn,
            CancellationToken cancellationToken)
        {
            return await _context.BlogsAuthors
                .AsNoTracking()
                .AnyAsync(x =>
                    x.Id != authorId &&
                    (
                        (HasValue(nameAr) && x.NameAr != null && x.NameAr.ToLower() == nameAr!.ToLowerInvariant()) ||
                        (HasValue(nameEn) && x.NameEn != null && x.NameEn.ToLower() == nameEn!.ToLowerInvariant())
                    ),
                    cancellationToken);
        }

        private async Task<bool> IsEmailTakenAsync(
            Guid authorId,
            string email,
            CancellationToken cancellationToken)
        {
            return await _context.BlogsAuthors
                .AsNoTracking()
                .AnyAsync(x =>
                    x.Id != authorId &&
                    x.Email != null &&
                    x.Email.ToLower() == email,
                    cancellationToken);
        }

        private async Task<bool> IsPhoneTakenAsync(
            Guid authorId,
            string phoneNumber,
            CancellationToken cancellationToken)
        {
            return await _context.BlogsAuthors
                .AsNoTracking()
                .AnyAsync(x =>
                    x.Id != authorId &&
                    x.PhoneNumber == phoneNumber,
                    cancellationToken);
        }

        private static bool HasValue(string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
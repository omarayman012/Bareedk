
using BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.File;
using MediatR;
using Microsoft.AspNetCore.Identity;
using BaridikExpress.Domain.Entities.BlogsModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Handler
{
    public class CreateBlogsAuthorHandler : IRequestHandler<CreateBlogsAuthorCommand, Result<ResponseBlogsAuthorDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IGetCurrentUserRepository _getCurrentUser;
        private readonly IFileStorageService _fileStorageService;
        private readonly IStringLocalizer _localizer;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CreateBlogsAuthorHandler> _logger;

        public CreateBlogsAuthorHandler(
            IApplicationDbContext context,
            IGetCurrentUserRepository getCurrentUser,
            IFileStorageService fileStorageService,
            IStringLocalizer localizer,
            UserManager<User> userManager,
            ILogger<CreateBlogsAuthorHandler> logger)
        {
            _context = context;
            _getCurrentUser = getCurrentUser;
            _fileStorageService = fileStorageService;
            _localizer = localizer;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<ResponseBlogsAuthorDto>> Handle(CreateBlogsAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _getCurrentUser.GetUserId();

                if (string.IsNullOrWhiteSpace(currentUserId))
                    return Result<ResponseBlogsAuthorDto>.Failure(_localizer["Unauthorized"], 401);

                var currentUser = await _userManager.FindByIdAsync(currentUserId);

                if (currentUser is null)
                    return Result<ResponseBlogsAuthorDto>.Failure(_localizer["UserNotFound"], 404);

                var nameAr = NormalizeNullable(request.NameAr);
                var nameEn = NormalizeNullable(request.NameEn);

                if (nameAr is null && nameEn is null)
                    return Result<ResponseBlogsAuthorDto>.Failure(_localizer["NameArOrNameEnRequired"], 400);

                if (nameAr is null)
                    nameAr = nameEn;

                if (nameEn is null)
                    nameEn = nameAr;

                var normalizedNameAr = NormalizeForComparison(nameAr!);
                var normalizedNameEn = NormalizeForComparison(nameEn!);
                var email = NormalizeRequired(request.Email);
                var normalizedEmail = NormalizeForComparison(email);
                var phoneNumber = NormalizeRequired(request.PhoneNumber);

                var category = await _context.BlogsCategorys
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.BlogsCategoryId, cancellationToken);

                if (category is null)
                    return Result<ResponseBlogsAuthorDto>.Failure(_localizer["CategoryNotFound"], 404);

                var authorNameExists = await _context.BlogsAuthors
                    .AsNoTracking()
                    .AnyAsync(x =>
                        (x.NameAr != null && x.NameAr.ToLower() == normalizedNameAr) ||
                        (x.NameEn != null && x.NameEn.ToLower() == normalizedNameEn),
                        cancellationToken);

                if (authorNameExists)
                    return Result<ResponseBlogsAuthorDto>.Failure(_localizer["AuthorNameAlreadyExists"], 400);

                var emailExists = await _context.BlogsAuthors
                    .AsNoTracking()
                    .AnyAsync(x => x.Email != null && x.Email.ToLower() == normalizedEmail, cancellationToken);

                if (emailExists)
                    return Result<ResponseBlogsAuthorDto>.Failure(_localizer["EmailAlreadyExists"], 400);

                var phoneExists = await _context.BlogsAuthors
                    .AsNoTracking()
                    .AnyAsync(x => x.PhoneNumber == phoneNumber, cancellationToken);

                if (phoneExists)
                    return Result<ResponseBlogsAuthorDto>.Failure(_localizer["PhoneNumberAlreadyExists"], 400);

                await using var stream = request.Image!.OpenReadStream();

                var imageUrl = await _fileStorageService.SaveFileAsync(
                    stream,
                    request.Image.FileName,
                    "BlogsAuthors");

                if (string.IsNullOrWhiteSpace(imageUrl))
                    return Result<ResponseBlogsAuthorDto>.Failure(_localizer["ImageUploadFailed"], 500);


                var author = new  BaridikExpress.Domain.Entities.BlogsModules.BlogsAuthor
                {
                    Id = Guid.NewGuid(),
                    NameAr = nameAr!,
                    NameEn = nameEn!,
                    Gender = request.Gender,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Image = imageUrl,
                    BlogsCategoryId = request.BlogsCategoryId,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = currentUserId
                };

                _context.BlogsAuthors.Add(author);
                await _context.SaveChangesAsync(cancellationToken);

                var response = new ResponseBlogsAuthorDto
                {
                    Id = author.Id,
                    Name = new LocalizedDto
                    {
                        AR = author.NameAr,
                        EN = author.NameEn
                    },
                    Gender = author.Gender.ToString(),
                    Email = author.Email,
                    PhoneNumber = author.PhoneNumber,
                    Image = author.Image,
                    BlogsCategoryName = new LocalizedNameDto
                    {
                        Id = category.Id,
                        AR = category.NameAr,
                        EN = category.NameEn
                    },
                    IsActive = author.IsActive,
                    CreatedBy = currentUser.FullName
                };

                return Result<ResponseBlogsAuthorDto>.Success(
                    response,
                    _localizer["AuthorCreatedSuccessfully"],
                    201);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating blog author");
                return Result<ResponseBlogsAuthorDto>.Error(_localizer["UnexpectedError"], 500);
            }
        }

        private static string NormalizeRequired(string value)
        {
            return value.Trim();
        }

        private static string? NormalizeNullable(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        private static string NormalizeForComparison(string value)
        {
            return value.Trim().ToLower();
        }
    }
}


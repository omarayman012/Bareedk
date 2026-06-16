
using BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;
using BaridikExpress.Application.Features.BlogsModules.Blogs.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Interfaces.BlogModules;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.BlogsModules;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules;

public class CreateBlogCommandHandler(
    IApplicationDbContext context,
    IBlogService blogService,
    IFileStorageService fileStorage,
    IStringLocalizer localizer,
    IGetCurrentUserRepository getCurrentUser,
    IMediator mediator)
    : IRequestHandler<CreateBlogCommand, Result<BlogDetailsResponse>>
{
    public async Task<Result<BlogDetailsResponse>> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var currentUserId = getCurrentUser.GetUserId();

            if (string.IsNullOrWhiteSpace(currentUserId))
                return Result<BlogDetailsResponse>.Failure(localizer["Unauthorized"], 401);

            var currentUser = await context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == currentUserId, cancellationToken);

            if (currentUser is null)
                return Result<BlogDetailsResponse>.Failure(localizer["UserNotFound"], 404);

            var titleAr = string.IsNullOrWhiteSpace(request.TitleAr)
                ? request.TitleEn?.Trim()
                : request.TitleAr.Trim();

            var titleEn = string.IsNullOrWhiteSpace(request.TitleEn)
                ? request.TitleAr?.Trim()
                : request.TitleEn.Trim();

            if (string.IsNullOrWhiteSpace(titleAr) && string.IsNullOrWhiteSpace(titleEn))
                return Result<BlogDetailsResponse>.Failure(localizer["TitleRequired"], 400);

            var descAr = string.IsNullOrWhiteSpace(request.DescriptionAr)
                ? request.DescriptionEn?.Trim()
                : request.DescriptionAr.Trim();

            var descEn = string.IsNullOrWhiteSpace(request.DescriptionEn)
                ? request.DescriptionAr?.Trim()
                : request.DescriptionEn.Trim();


            var validationResult = await ValidateRequestAsync(request, cancellationToken);

            if (validationResult is not null)
                return Result<BlogDetailsResponse>.Failure(
                    validationResult.Message,
                    validationResult.StatusCode);

            var imageUrl = await UploadImageAsync(request.Image);

            var blog = BuildBlog(
                request,
                imageUrl,
                currentUserId,
                titleAr!,
                titleEn!,
                descAr!,
                descEn!);

            blog.BlogTags = await blogService.HandleTagsAsync(request.Tags, blog.Id);

            request.Seo ??= new CreateBlogSeoDto();

            request.Seo.SlugEn = request.Seo.SlugEn.Any()
                ? request.Seo.SlugEn
                : new List<string> { titleEn! };

            request.Seo.SlugAr = request.Seo.SlugAr.Any()
                ? request.Seo.SlugAr
                : new List<string> { titleAr! };

            blog.Seo = await blogService.HandleSeoAsync(request.Seo, blog);

            context.Blogs.Add(blog);

            await context.SaveChangesAsync(cancellationToken);

            var blogResult = await mediator.Send(new GetBlogByIdQuery
            {
                Id = blog.Id,
                CommentsPageNumber = 1,
                CommentsPageSize = 5
            }, cancellationToken);

            if (!blogResult.IsSuccess)
                return blogResult;

            return Result<BlogDetailsResponse>.Success(
                blogResult.Data!,
                localizer["CreateBlogSuccessfully"],
                201);
        }
        catch (Exception ex)
        {
            return Result<BlogDetailsResponse>.Error(
                localizer["UnexpectedError"] + $": {ex.Message}",
                500);
        }
    }

    private async Task<Result<Guid>?> ValidateRequestAsync(
        CreateBlogCommand request,
        CancellationToken cancellationToken)
    {
        if (!await context.BlogsAuthors.AnyAsync(
                x => x.Id == request.BlogsAuthorId,
                cancellationToken))
        {
            return Result<Guid>.Failure(localizer["BlogAuthorNotFound"], 400);
        }

        if (!await context.BlogsCategorys.AnyAsync(
                x => x.Id == request.BlogsCategoryId,
                cancellationToken))
        {
            return Result<Guid>.Failure(localizer["BlogCategoryNotFound"], 400);
        }

        return null;
    }

    private static Blog BuildBlog(
        CreateBlogCommand request,
        string imageUrl,
        string currentUserId,
        string titleAr,
        string titleEn,
        string descAr,
        string descEn) => new()
        {
            Id = Guid.NewGuid(),
            TitleAr = titleAr,
            TitleEn = titleEn,
            DescriptionAr = descAr,
            DescriptionEn = descEn,
            BlogsCategoryId = request.BlogsCategoryId,
            BlogsAuthorId = request.BlogsAuthorId,
            PublishingHouseId = request.PublishingHouseId,
            CreatedDate = request.CreatedDate,
            CreatedTime = request.CreatedTime,
            Image = imageUrl,
            IsActive = request.IsActive,
            CreatedById = currentUserId,
            CreatedAt = DateTime.UtcNow
        };

    private async Task<string> UploadImageAsync(IFormFile? file)
    {
        if (file is null || file.Length == 0)
            return string.Empty;

        await using var stream = file.OpenReadStream();

        return await fileStorage.SaveFileAsync(
                   stream,
                   file.FileName,
                   "blogs")
               ?? string.Empty;
    }
}

using BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;
using BaridikExpress.Application.Interfaces.BlogModules;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.BlogsModules;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogModules;

public class UpdateBlogCommandHandler(
    IApplicationDbContext context,
    IBlogService blogService,
    IFileStorageService fileStorage,
    IStringLocalizer localizer)
    : IRequestHandler<UpdateBlogCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        var blog = await context.Blogs
            .Include(x => x.BlogTags)
            .Include(x => x.Seo)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (blog is null)
            return Result<bool>.Failure(localizer["BlogNotFound"], 404);

        UpdateBasicInfo(blog, request);

        await UpdateImageAsync(blog, request, cancellationToken);
        await UpdateTagsAsync(blog, request, cancellationToken);
        await UpdateSeoAsync(blog, request, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, localizer["BlogUpdatedSuccessfully"]);
    }

    private static void UpdateBasicInfo(dynamic blog, UpdateBlogCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.TitleAr))
            blog.TitleAr = request.TitleAr.Trim();

        if (!string.IsNullOrWhiteSpace(request.TitleEn))
            blog.TitleEn = request.TitleEn.Trim();

        if (!string.IsNullOrWhiteSpace(request.DescriptionAr))
            blog.DescriptionAr = request.DescriptionAr.Trim();

        if (!string.IsNullOrWhiteSpace(request.DescriptionEn))
            blog.DescriptionEn = request.DescriptionEn.Trim();

        if (request.BlogsCategoryId.HasValue)
            blog.BlogsCategoryId = request.BlogsCategoryId.Value;

        if (request.BlogsAuthorId.HasValue)
            blog.BlogsAuthorId = request.BlogsAuthorId.Value;

        if (request.PublishingHouseId.HasValue)
            blog.PublishingHouseId = request.PublishingHouseId.Value;

        if (request.IsActive.HasValue)
            blog.IsActive = request.IsActive.Value;

        if (request.CreatedDate != default)
            blog.CreatedDate = request.CreatedDate;

        if (request.CreatedTime != default)
            blog.CreatedTime = request.CreatedTime;
    }

    private async Task UpdateImageAsync(dynamic blog, UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        if (request.Image is null)
            return;

        await using var stream = request.Image.OpenReadStream();

        var updatedImage = await fileStorage.UpdateFileAsync(
            stream,
            request.Image.FileName,
            blog.Image,
            "blogs");

        if (!string.IsNullOrWhiteSpace(updatedImage))
            blog.Image = updatedImage;
    }

    private async Task UpdateTagsAsync(dynamic blog, UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        if (request.Tags is null)
            return;

        context.BlogTags.RemoveRange(blog.BlogTags);

        if (request.Tags.Any())
        {
            blog.BlogTags = await blogService.HandleTagsAsync(request.Tags, blog.Id);
        }
        else
        {
            blog.BlogTags = new List<BlogTag>();
        }
    }

    private async Task UpdateSeoAsync(dynamic blog, UpdateBlogCommand request, CancellationToken cancellationToken)
    {
        if (request.Seo is null)
            return;

        if (blog.Seo is null)
        {
            blog.Seo = await blogService.HandleSeoAsync(request.Seo, blog);
            return;
        }

        if (!string.IsNullOrWhiteSpace(request.Seo.MetaTitleAr))
            blog.Seo.MetaTitleAr = request.Seo.MetaTitleAr.Trim();

        if (!string.IsNullOrWhiteSpace(request.Seo.MetaTitleEn))
            blog.Seo.MetaTitleEn = request.Seo.MetaTitleEn.Trim();

        if (!string.IsNullOrWhiteSpace(request.Seo.MetaDescriptionAr))
            blog.Seo.MetaDescriptionAr = request.Seo.MetaDescriptionAr.Trim();

        if (!string.IsNullOrWhiteSpace(request.Seo.MetaDescriptionEn))
            blog.Seo.MetaDescriptionEn = request.Seo.MetaDescriptionEn.Trim();

        if (request.Seo.MetaKeywordsAr is not null && request.Seo.MetaKeywordsAr.Any())
            blog.Seo.MetaKeywordsAr = string.Join(",", request.Seo.MetaKeywordsAr);

        if (request.Seo.MetaKeywordsEn is not null && request.Seo.MetaKeywordsEn.Any())
            blog.Seo.MetaKeywordsEn = string.Join(",", request.Seo.MetaKeywordsEn);

        if (request.Seo.SlugAr is not null && request.Seo.SlugAr.Any())
            blog.Seo.SlugAr = string.Join(",", request.Seo.SlugAr);

        if (request.Seo.SlugEn is not null && request.Seo.SlugEn.Any())
            blog.Seo.SlugEn = string.Join(",", request.Seo.SlugEn);

        if (request.Seo.MetaImage is not null)
        {
            var seo = await blogService.HandleSeoAsync(request.Seo, blog);
            if (!string.IsNullOrWhiteSpace(seo.MetaImage))
                blog.Seo.MetaImage = seo.MetaImage;
        }
    }
}
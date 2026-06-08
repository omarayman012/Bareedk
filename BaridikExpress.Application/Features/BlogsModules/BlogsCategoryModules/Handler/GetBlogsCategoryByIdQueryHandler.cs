
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules
{
    public class GetBlogsCategoryByIdQueryHandler
        : IRequestHandler<GetBlogsCategoryByIdQuery, Result<GetBlogsCategoryByIdDto>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IStringLocalizer _localizer;

        public GetBlogsCategoryByIdQueryHandler(
            IApplicationDbContext applicationDbContext,
            IStringLocalizer localizer)
        {
            _applicationDbContext = applicationDbContext;
            _localizer = localizer;
        }

        public async Task<Result<GetBlogsCategoryByIdDto>> Handle(
            GetBlogsCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var blogsCategory = await _applicationDbContext.BlogsCategorys
                    .AsNoTracking()
                    .FirstOrDefaultAsync(category => category.Id == request.Id, cancellationToken);

                if (blogsCategory == null)
                {
                    return Result<GetBlogsCategoryByIdDto>.Failure(
                        _localizer["BlogsCategoryNotFound"],
                        404);
                }

                string? createdByName = null;
                string? updatedByName = null;

                if (!string.IsNullOrWhiteSpace(blogsCategory.CreatedById))
                {
                    createdByName = await _applicationDbContext.ApplicationUsers
                        .AsNoTracking()
                        .Where(user => user.Id == blogsCategory.CreatedById)
                        .Select(user => user.FullName)
                        .FirstOrDefaultAsync(cancellationToken);
                }

                if (!string.IsNullOrWhiteSpace(blogsCategory.UpdatedById))
                {
                    updatedByName = await _applicationDbContext.ApplicationUsers
                        .AsNoTracking()
                        .Where(user => user.Id == blogsCategory.UpdatedById)
                        .Select(user => user.FullName)
                        .FirstOrDefaultAsync(cancellationToken);
                }

                var blogs = await _applicationDbContext.Blogs
                    .AsNoTracking()
                    .Where(blog => blog.BlogsCategoryId == blogsCategory.Id)
                    .Select(blog => new BlogBasicDto
                    {
                        Id = blog.Id,
                        Title = new LocalizedDto
                        {
                            AR = blog.TitleAr ?? string.Empty,
                            EN = blog.TitleEn ?? string.Empty
                        },
                        Image = blog.Image,
                        IsActive = blog.IsActive
                    })
                    .ToListAsync(cancellationToken);

                var blogsCategoryDto = new GetBlogsCategoryByIdDto
                {
                    Id = blogsCategory.Id,
                    Name = new LocalizedDto
                    {
                        AR = blogsCategory.NameAr ?? string.Empty,
                        EN = blogsCategory.NameEn ?? string.Empty
                    },
                    Priorty = blogsCategory.Priorty,
                    Description = new LocalizedDto
                    {
                        AR = blogsCategory.DescriptionAr,
                        EN = blogsCategory.DescriptionEn
                    },
                    Image = blogsCategory.Image,
                    IsActive = blogsCategory.IsActive,
                    BlogsCount = blogs.Count,
                    Blogs = blogs,
                    CreatedAt = blogsCategory.CreatedAt,
                    CreatedBy = createdByName,
                    UpdatedAt = blogsCategory.UpdatedAt,
                    UpdatedBy = updatedByName
                };

                return Result<GetBlogsCategoryByIdDto>.Success(
                    blogsCategoryDto,
                    _localizer["BlogsCategoryRetrievedSuccessfully"],
                    200);
            }
            catch (Exception)
            {
                return Result<GetBlogsCategoryByIdDto>.Failure(
                    _localizer["FailedToRetrieveBlogsCategory"],
                    500);
            }
        }
    }
}
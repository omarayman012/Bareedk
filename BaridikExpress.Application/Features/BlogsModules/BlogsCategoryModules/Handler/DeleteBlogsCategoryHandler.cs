
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules
{
    public class DeleteBlogsCategoryHandler
        : IRequestHandler<DeleteBlogsCategoryCommand, Result<bool>>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IStringLocalizer _localizer;

        public DeleteBlogsCategoryHandler(
            IApplicationDbContext applicationDbContext,
            IStringLocalizer localizer)
        {
            _applicationDbContext = applicationDbContext;
            _localizer = localizer;
        }

        public async Task<Result<bool>> Handle(
            DeleteBlogsCategoryCommand request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.Ids == null || !request.Ids.Any())
                {
                    return Result<bool>.Failure(
                        _localizer["IdsRequired"],
                        400);
                }

                var blogsCategoriesToDelete = await _applicationDbContext.BlogsCategorys
                    .Where(category => request.Ids.Contains(category.Id))
                    .ToListAsync(cancellationToken);

                if (!blogsCategoriesToDelete.Any())
                {
                    return Result<bool>.Failure(
                        _localizer["BlogsCategoryNotFound"],
                        404);
                }

                var blogsCategoryIdsToDelete = blogsCategoriesToDelete
                    .Select(category => category.Id)
                    .ToList();

                var hasRelatedBlogs = await HasRelatedBlogsAsync(
                    blogsCategoryIdsToDelete,
                    cancellationToken);

                if (hasRelatedBlogs)
                {
                    return Result<bool>.Failure(
                        _localizer["CannotDeleteBlogsCategoryBecauseItHasBlogs"],
                        400);
                }

                var hasRelatedBlogAuthors = await HasRelatedBlogAuthorsAsync(
                    blogsCategoryIdsToDelete,
                    cancellationToken);

                if (hasRelatedBlogAuthors)
                {
                    return Result<bool>.Failure(
                        _localizer["CannotDeleteBlogsCategoryBecauseItHasBlogAuthors"],
                        400);
                }

                _applicationDbContext.BlogsCategorys.RemoveRange(blogsCategoriesToDelete);

                await _applicationDbContext.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(
                    true,
                    _localizer["BlogsCategoryDeletedSuccessfully"],
                    200);
            }
            catch (Exception)
            {
                return Result<bool>.Error(
                    _localizer["UnexpectedError"],
                    500);
            }
        }

        private async Task<bool> HasRelatedBlogsAsync(
            List<Guid> blogsCategoryIdsToDelete,
            CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Blogs
                .AnyAsync(
                    blog => blogsCategoryIdsToDelete.Contains(blog.BlogsCategoryId),
                    cancellationToken);
        }

        private async Task<bool> HasRelatedBlogAuthorsAsync(
            List<Guid> blogsCategoryIdsToDelete,
            CancellationToken cancellationToken)
        {
            return await _applicationDbContext.BlogsAuthors
                .AnyAsync(
                    blogAuthor => blogsCategoryIdsToDelete.Contains(blogAuthor.BlogsCategoryId),
                    cancellationToken);
        }
    }
}
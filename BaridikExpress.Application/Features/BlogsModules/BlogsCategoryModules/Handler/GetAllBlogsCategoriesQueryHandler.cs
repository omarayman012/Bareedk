
using BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Domain.Entities.BlogsModules;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Awael_Al_Joudah.Application.Handlers.BlogsCategoryModules;

public class GetAllBlogsCategoriesQueryHandler(
    IApplicationDbContext applicationDbContext,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllBlogsCategoriesQuery, Result<PaginatedList<GetAllBlogsCategoryDto>>>
{
    public async Task<Result<PaginatedList<GetAllBlogsCategoryDto>>> Handle(
        GetAllBlogsCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var pageSize = request.pageSize ?? 10;
            string? createdByUserId = null;

            if (!string.IsNullOrWhiteSpace(request.CreatedById))
            {
                createdByUserId = await applicationDbContext.SubAdminEmployees
                    .AsNoTracking()
                    .Where(employee => employee.Id.ToString() == request.CreatedById)
                    .Select(employee => employee.UserId)
                    .FirstOrDefaultAsync(cancellationToken);
            }

            var blogsCategoriesQuery = applicationDbContext.BlogsCategorys
                .AsNoTracking()
                .AsQueryable();

            blogsCategoriesQuery = ApplyFilters(
                blogsCategoriesQuery,
                request,
                createdByUserId);

            var blogsCategoriesDtoQuery = blogsCategoriesQuery
                .OrderBy(category => category.Priorty ?? int.MaxValue)
                .Select(category => new GetAllBlogsCategoryDto
                {
                    Id = category.Id,
                    Name = new LocalizedDto
                    {
                        AR = category.NameAr ?? string.Empty,
                        EN = category.NameEn ?? string.Empty
                    },
                    Priorty = category.Priorty,
                    Description = new LocalizedDto
                    {
                        AR = category.DescriptionAr,
                        EN = category.DescriptionEn
                    },
                    Image = category.Image,
                    IsActive = category.IsActive,
                    BlogsCount = category.Blogs.Count(),
                    CreatedAt = category.CreatedAt,
                    CreatedBy = category.CreatedBy != null
                        ? category.CreatedBy.FullName
                        : null,
                    UpdatedAt = category.UpdatedAt,
                    UpdatedBy = category.UpdatedBy != null
                        ? category.UpdatedBy.FullName
                        : null
                });

            var paginatedBlogsCategories = await PaginatedList<GetAllBlogsCategoryDto>.CreateAsync(
                blogsCategoriesDtoQuery,
                request.PageNumber,
                pageSize);

            return Result<PaginatedList<GetAllBlogsCategoryDto>>.Success(
                paginatedBlogsCategories,
                localizer["BlogsCategoriesRetrievedSuccessfully"],
                200);
        }
        catch
        {
            return Result<PaginatedList<GetAllBlogsCategoryDto>>.Error(
                localizer["FailedToRetrieveBlogsCategories"],
                500);
        }
    }

    private static IQueryable<BlogsCategory> ApplyFilters(
        IQueryable<BlogsCategory> blogsCategoriesQuery,
        GetAllBlogsCategoriesQuery request,
        string? createdByUserId)
    {
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var searchName = request.Name.Trim().ToLower();

            blogsCategoriesQuery = blogsCategoriesQuery.Where(category =>
                (category.NameAr != null && category.NameAr.ToLower().Contains(searchName)) ||
                (category.NameEn != null && category.NameEn.ToLower().Contains(searchName)));
        }

        if (!string.IsNullOrWhiteSpace(createdByUserId))
        {
            blogsCategoriesQuery = blogsCategoriesQuery
                .Where(category => category.CreatedById == createdByUserId);
        }

        if (request.IsActive.HasValue)
        {
            blogsCategoriesQuery = blogsCategoriesQuery
                .Where(category => category.IsActive == request.IsActive.Value);
        }

        if (request.FromDate.HasValue)
        {
            blogsCategoriesQuery = blogsCategoriesQuery
                .Where(category => category.CreatedAt >= request.FromDate.Value);
        }

        if (request.ToDate.HasValue)
        {
            var toDateExclusive = request.ToDate.Value.Date.AddDays(1);

            blogsCategoriesQuery = blogsCategoriesQuery
                .Where(category => category.CreatedAt < toDateExclusive);
        }

        return blogsCategoriesQuery;
    }
}
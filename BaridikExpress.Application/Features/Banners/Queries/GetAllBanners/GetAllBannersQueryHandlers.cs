using BaridikExpress.Application.Common.Extensions;
using BaridikExpress.Application.Features.Banners.DTO;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Banners;

namespace BaridikExpress.Application.Features.Banners.Queries.GetAllBanners;

public sealed class GetAllBannersQueryHandler(
    IGenericRepository<Banner> repo,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllBannersQuery, Result<PaginatedList<GetAllBannersDto>>>
{
    public async Task<Result<PaginatedList<GetAllBannersDto>>> Handle(
        GetAllBannersQuery request,
        CancellationToken cancellationToken)
    {
        var query = repo.Query();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(x =>
                x.TitleEn.Contains(request.Name) ||
                x.TitleAr.Contains(request.Name) ||
                x.DescriptionEn.Contains(request.Name) ||
                x.DescriptionAr.Contains(request.Name));

        query = query.ApplyCommonFilters(request);

        var result = query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllBannersDto
            {
                Id = x.Id,
                Title = new LocalizedDto
                {
                    EN = x.TitleEn,
                    AR = x.TitleAr
                },
                Description = new LocalizedDto
                {
                    EN = x.DescriptionEn,
                    AR = x.DescriptionAr
                },
                ImageUrl = x.ImageUrl,
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : "",
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FullName : "",
                UpdatedAt = x.UpdatedAt
            });

        var paginatedResult = await PaginatedList<GetAllBannersDto>
            .CreateAsync(result, request.PageNumber, request.PageSize);

        return Result<PaginatedList<GetAllBannersDto>>
            .Success(paginatedResult, localizer["OperationCompletedSuccessfully"]);
    }
}
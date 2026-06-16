using BaridikExpress.Application.Features.Banners.DTO;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Banners;

namespace BaridikExpress.Application.Features.Banners.Queries.GetAllBannersMobile;

public sealed class GetAllBannersMobileQueryHandler(
    IGenericRepository<Banner> repo,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllBannersMobileQuery, Result<PaginatedList<GetAllBannersMobileDto>>>
{
    public async Task<Result<PaginatedList<GetAllBannersMobileDto>>> Handle(
        GetAllBannersMobileQuery request,
        CancellationToken cancellationToken)
    {
        var query = repo.Query()
            .Where(x => x.IsActive)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new GetAllBannersMobileDto
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
                ImageUrl = x.ImageUrl
            });

        var result = await PaginatedList<GetAllBannersMobileDto>
            .CreateAsync(
                query,
                request.PageNumber,
                request.PageSize);

        return Result<PaginatedList<GetAllBannersMobileDto>>
            .Success(
                result,
                localizer["OperationCompletedSuccessfully"]);
    }
}
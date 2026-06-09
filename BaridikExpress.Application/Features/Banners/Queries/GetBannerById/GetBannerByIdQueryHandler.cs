using BaridikExpress.Application.Features.Banners.DTO;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.Banners;
namespace BaridikExpress.Application.Features.Banners.Queries.GetBannerById
{
    public class GetBannerByIdQueryHandler(
        IGenericRepository<Banner> repo,
        IStringLocalizer localizer)
        : IRequestHandler<GetBannerByIdQuery, Result<GetBannerByIdDto>>
    {
        private readonly IGenericRepository<Banner> _repo = repo;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<GetBannerByIdDto>> Handle(
            GetBannerByIdQuery request,
            CancellationToken cancellationToken)
        {
            var banner = await _repo.Query()
        .Include(x => x.CreatedBy)
        .Include(x => x.UpdatedBy)
        .Where(x => x.Id == request.Id)
        .Select(x => new GetBannerByIdDto
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
            CreatedBy = x.CreatedBy != null
                ? x.CreatedBy.FullName
                : string.Empty,
            CreatedAt = x.CreatedAt,
            UpdatedBy = x.UpdatedBy != null
                ? x.UpdatedBy.FullName
                : string.Empty,

            UpdatedAt = x.UpdatedAt
        })
        .FirstOrDefaultAsync(cancellationToken);

            if (banner is null)
                return Result<GetBannerByIdDto>
                    .Failure(_localizer["BannerNotFound"], 404);

            return Result<GetBannerByIdDto>
                .Success(
                    banner,
                    _localizer["OperationCompletedSuccessfully"]);
        }
    }
}


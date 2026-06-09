using BaridikExpress.Application.Features.Banners.DTO;

namespace BaridikExpress.Application.Features.Banners.Queries.GetBannerById
{
    public record GetBannerByIdQuery(Guid Id)
        : IRequest<Result<GetBannerByIdDto>>;
}

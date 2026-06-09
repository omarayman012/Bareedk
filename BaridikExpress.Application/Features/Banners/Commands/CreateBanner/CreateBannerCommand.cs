namespace BaridikExpress.Application.Features.Banners.Commands.CreateBanner;

public record CreateBannerCommand(
    string? TitleAr,
    string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    IFormFile Image) : IRequest<Result<Guid>>;

using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.Banners.Commands.UpdateBanner;

public record UpdateBannerCommand(
    Guid Id,
    string? TitleAr,
    string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    IFormFile? Image) : IRequest<Result<bool>>;

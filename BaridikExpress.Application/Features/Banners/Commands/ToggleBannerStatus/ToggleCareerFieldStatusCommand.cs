namespace BaridikExpress.Application.Features.Banners.Commands.ToggleBannerStatus;

public record ToggleBannerStatusCommand(
    Guid Id
) : IRequest<Result<bool>>;
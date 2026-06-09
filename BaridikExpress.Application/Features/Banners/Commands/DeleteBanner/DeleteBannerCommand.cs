namespace BaridikExpress.Application.Features.Banners.Commands.DeleteBanner;

public record DeleteBannerCommand(List<Guid> Ids) : IRequest<Result<bool>>;

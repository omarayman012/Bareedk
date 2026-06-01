namespace BaridikExpress.Application.Interfaces.Services;

public interface IMapService
{
    Task<string?> GetAddressFromCoordinatesAsync(
        decimal latitude,
        decimal longitude,
        CancellationToken cancellationToken = default);
}

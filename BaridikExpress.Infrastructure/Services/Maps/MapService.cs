using BaridikExpress.Application.Interfaces.Services;

namespace BaridikExpress.Infrastructure.Services.Maps
{
    public class MapService : IMapService
    {
        public Task<string?> GetAddressFromCoordinatesAsync(decimal latitude, decimal longitude, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}

using System.Text.Json;
using BaridikExpress.Application.Interfaces.Services;
namespace BaridikExpress.Infrastructure.Services.Maps;
public class MapService : IMapService
{
    private readonly HttpClient _httpClient;
    public MapService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("BaridikExpress");
    }

    public async Task<string?> GetAddressFromCoordinatesAsync(
        decimal latitude,
        decimal longitude,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var url =$"https://nominatim.openstreetmap.org/reverse?lat={latitude}&lon={longitude}&format=jsonv2";

            var response = await _httpClient.GetAsync( url, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            using var document = JsonDocument.Parse(json);
            if (document.RootElement.TryGetProperty("display_name", out var displayName))
                return displayName.GetString();

            return null;
        }
        catch
        {
            return null;
        }
    }
}
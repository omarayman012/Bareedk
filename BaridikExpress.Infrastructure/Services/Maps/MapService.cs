using BaridikExpress.Application.Interfaces.Services;
using BaridikExpress.Infrastructure.Services.Maps;
using Microsoft.Extensions.Options;
using System.Text.Json;

public sealed class GoogleGeocodingService : IMapService
{
    private readonly HttpClient _httpClient;
    private readonly GoogleMapsOptions _options;

    public GoogleGeocodingService(
        HttpClient httpClient,
        IOptions<GoogleMapsOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string?> GetAddressFromCoordinatesAsync(
        decimal latitude,
        decimal longitude,
        CancellationToken cancellationToken = default)
    {
        var url =
            $"https://maps.googleapis.com/maps/api/geocode/json" +
            $"?latlng={latitude},{longitude}" +
            $"&key={_options.ApiKey}";

        var response =
            await _httpClient.GetAsync(
                url,
                cancellationToken);

        if (!response.IsSuccessStatusCode)
            return null;

        var json =
            await response.Content.ReadAsStringAsync(
                cancellationToken);

        using var document =
            JsonDocument.Parse(json);

        var status =
            document.RootElement
                .GetProperty("status")
                .GetString();

        if (status != "OK")
            return null;

        var results =
            document.RootElement
                .GetProperty("results");

        if (results.GetArrayLength() == 0)
            return null;

        return results[0]
            .GetProperty("formatted_address")
            .GetString();
    }
}
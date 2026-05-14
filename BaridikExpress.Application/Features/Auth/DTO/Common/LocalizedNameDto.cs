using System.Text.Json.Serialization;

namespace BaridikExpress.Application.Features.Auth.DTO.Common;
public sealed class LocalizedNameDto
{
    [JsonPropertyName("En")]
    public string En { get; set; } = string.Empty;

    [JsonPropertyName("Ar")]
    public string Ar { get; set; } = string.Empty;

    public static LocalizedNameDto FromEnAr(string en, string ar) =>
        new() { En = en ?? string.Empty, Ar = ar ?? string.Empty };
}

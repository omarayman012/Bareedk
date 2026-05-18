using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.LocationGeography.Dto.Village;

public class VillageDto
{
    public Guid Id { get; set; }
    public LocalizedDto?Name { get; set; }
    public LocalizedNameDto? CityName { get; set; }
    public LocalizedNameDto? GovernmentName { get; set; }
    public LocalizedNameDto? CountryName { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }

}

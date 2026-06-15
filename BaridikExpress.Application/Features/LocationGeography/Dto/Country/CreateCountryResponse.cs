using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.LocationGeography.Dto.Country;

public class CreateCountryResponse
{
    public Guid Id { get; set; }
    public LocalizedDto Name { get; set; } = new();
    public string PhoneCode { get; set; } = string.Empty;
    public string? PostalCode { get; set; } 
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
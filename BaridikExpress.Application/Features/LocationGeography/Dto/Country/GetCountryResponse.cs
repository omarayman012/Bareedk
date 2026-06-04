using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.LocationGeography.Dto.Country;

public class GetCountryResponse
{
    public Guid Id { get; set; }
    public LocalizedDto Name { get; set; } = new LocalizedDto();
    public string PhoneCode { get; set; } = string.Empty;
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
}

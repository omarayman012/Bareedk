using BaridikExpress.Application.Common.Helpers;

namespace BaridikExpress.Application.Features.CustomerAddresses.DTOs;

public class GetAllAddressesDto
{
    public Guid Id { get; set; }
    public string? RecipientName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? AddressType { get; set; }
    public LocalizedEntityDto? Country { get; set; }
    public LocalizedEntityDto? Government { get; set; }
    public LocalizedEntityDto? City { get; set; }
    public LocalizedEntityDto? Village { get; set; }
    public string? Street { get; set; }
    public string? BuildingNumber { get; set; }
    public string? ApartmentNumber { get; set; }
    public string? FloorNumber { get; set; }
    public string? DistinctiveMark { get; set; }
    public string? ZipCode { get; set; }
    public string? Location { get; set; }
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public bool IsDefault { get; set; }
    public string CreatedBy { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

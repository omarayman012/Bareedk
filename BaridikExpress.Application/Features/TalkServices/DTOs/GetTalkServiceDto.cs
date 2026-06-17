using BaridikExpress.Application.Common.Helpers;

namespace BaridikExpress.Application.Features.TalkServices.DTOs;

public sealed class GetTalkServiceDto
{
    public Guid Id { get; set; }

    public List<LocalizedEntityDto> Services { get; set; } = [];

    public string ShipmentVolumeRange { get; set; } = default!;

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    public LocalizedEntityDto Country { get; set; } = default!;
    public LocalizedEntityDto Government { get; set; } = default!;
    public LocalizedEntityDto? City { get; set; }
    public LocalizedEntityDto? Village { get; set; }

    public string PostalCode { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public string WorkEmail { get; set; } = default!;
    public string JobTitle { get; set; } = default!;
    public string CompanyName { get; set; } = default!;
    public string CompanyAddress { get; set; } = default!;
    public string WebsiteUrl { get; set; } = default!;

    public string RequiredDetails { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = default!;
}
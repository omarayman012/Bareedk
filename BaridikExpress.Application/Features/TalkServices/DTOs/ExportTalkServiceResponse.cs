namespace BaridikExpress.Application.Features.TalkServices.DTOs;

public sealed class ExportTalkServiceResponse
{
    public string Services { get; set; } = default!;

    public string ShipmentVolumeRange { get; set; } = default!;

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Country { get; set; } = default!;

    public string Government { get; set; } = default!;

    public string? City { get; set; }

    public string? Village { get; set; }

    public string PostalCode { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public string WorkEmail { get; set; } = default!;

    public string JobTitle { get; set; } = default!;

    public string CompanyName { get; set; } = default!;

    public string CompanyAddress { get; set; } = default!;

    public string WebsiteUrl { get; set; } = default!;

    public string RequiredDetails { get; set; } = default!;

    public string Status { get; set; } = default!;

    public DateTime CreatedAt { get; set; }
}
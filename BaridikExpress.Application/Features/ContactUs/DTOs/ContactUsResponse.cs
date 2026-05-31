namespace BaridikExpress.Application.Features.ContactUs.DTOs;

public sealed record ContactUsResponse(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    string Message,
    bool IsRead,
    DateTime CreatedAt
);

namespace BaridikExpress.Application.Features.Customer.Dtos;

public record CreateContactDto(
    string PhoneCountryCode,
    string PhoneNumber,
    string? Email,
    string? WhatsAppCountryCode,
    string? WhatsAppNumber,
    bool IsPrimary
);
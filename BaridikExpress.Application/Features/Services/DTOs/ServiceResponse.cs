using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Services.DTOs;
public sealed record ServiceResponse(
    Guid Id,
    LocalizedDto Name,//NAME {AR:   ,EN: }
    decimal Price,
    string Currency,
    string? Image,
    bool IsActive,
    string? CreatedBy,
    DateTime CreatedAt,
    string? UpdatedBy,
    DateTime? UpdatedAt
)
{
    public string PriceWithCurrency => $"{Price} {Currency}";
}
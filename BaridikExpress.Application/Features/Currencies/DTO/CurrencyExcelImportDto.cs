namespace BaridikExpress.Application.Features.Currencies.DTO;

public sealed class CurrencyExcelImportDto
{
    public string? NameEn { get; set; }
    public string? NameAr { get; set; }
    public string CurrencyCode { get; set; } = default!;
    public string? CurrencySymbol { get; set; }
    public bool IsActive { get; set; } = true;
}
namespace BaridikExpress.Application.Features.Currencies.DTO;


[AttributeUsage(AttributeTargets.Property)]
public sealed class ExcelDateFormatAttribute : Attribute
{
    public string Format { get; }
    public ExcelDateFormatAttribute(string format = "yyyy-MM-dd hh:mm tt")
        => Format = format;
}

public sealed class CurrencyExcelExportDto
{
    public string NameEn { get; set; } = default!;
    public string NameAr { get; set; } = default!;
    public string CurrencyCode { get; set; } = default!;
    public string? CurrencySymbol { get; set; }
    public bool IsActive { get; set; }
    public string? CreatedBy { get; set; }
    [ExcelDateFormat("yyyy-MM-dd hh:mm tt")]
    public DateTime CreatedAt { get; set; }
    public string? UpdatedBy { get; set; }
    [ExcelDateFormat("yyyy-MM-dd hh:mm tt")]
    public DateTime? UpdatedAt { get; set; }
}
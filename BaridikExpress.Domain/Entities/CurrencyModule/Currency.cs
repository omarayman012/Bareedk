using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.ValueObjects;

namespace BaridikExpress.Domain.Entities.CurrencyModule;

public class Currency : BaseEntity
{
    public Guid Id { get; private set; }
    public LocalizedString Name { get; private set; } = default!;
    public string CurrencyCode { get; private set; } = string.Empty;
    public string? CurrencySymbol { get; private set; }

    private Currency() { }

    public Currency(string? nameEn, string? nameAr, string currencyCode, string? currencySymbol)
    {
        Id = Guid.NewGuid();
        SetName(nameEn, nameAr);
        CurrencyCode = currencyCode.Trim();
        CurrencySymbol = currencySymbol?.Trim();
    }

    public void Update(string? nameEn, string? nameAr, string currencyCode, string? currencySymbol)
    {
        SetName(nameEn, nameAr);
        CurrencyCode = currencyCode.Trim();
        CurrencySymbol = currencySymbol?.Trim();
    }

    private void SetName(string? nameEn, string? nameAr)
    {
        Name = LocalizedString.Create(nameEn, nameAr);
    }
}
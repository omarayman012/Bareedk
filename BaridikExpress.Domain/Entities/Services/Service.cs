using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Services;

public class Service : BaseEntity
{
    public Guid Id { get; private set; }
    public string NameEn { get; private set; } = string.Empty;
    public string NameAr { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }

    private Service() { }

    public static Service Create(
        string nameEn,
        string nameAr,
        decimal price,
        string? imageUrl = null)
    {
        return new Service
        {
            Id = Guid.NewGuid(),
            NameEn = nameEn,
            NameAr = nameAr,
            Price = price,
            ImageUrl = imageUrl,
        };
    }

    public void Update(
        string? nameEn = null,
        string? nameAr = null,
        decimal? price = null,
        string? imageUrl = null)
    {
        if (!string.IsNullOrWhiteSpace(nameEn)) NameEn = nameEn;
        if (!string.IsNullOrWhiteSpace(nameAr)) NameAr = nameAr;
        if (price.HasValue) Price = price.Value;
        if (!string.IsNullOrWhiteSpace(imageUrl)) ImageUrl = imageUrl;
    }

}
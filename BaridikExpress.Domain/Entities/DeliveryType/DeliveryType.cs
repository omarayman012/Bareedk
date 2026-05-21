using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.DeliveryType;

public class DeliveryType : BaseEntity
{
    public Guid Id { get; private set; }
    public string NameEn { get; private set; } = string.Empty;
    public string NameAr { get; private set; } = string.Empty;
    public int DaysFrom { get; private set; }
    public int DaysTo { get; private set; }
    public decimal PricePerShipment { get; private set; }
    public bool IsSwitchActive { get; private set; }
    public string? ImageUrl { get; private set; }
    public string? Description { get; private set; }

    public decimal PricePerTotal => DaysTo * PricePerShipment;

    private DeliveryType() { }

    public static DeliveryType Create(
        string nameEn,
        string nameAr,
        int daysFrom,
        int daysTo,
        decimal pricePerShipment,
        bool isSwitchActive,
        string? imageUrl = null,
        string? description = null)
    {
        return new DeliveryType
        {
            Id = Guid.NewGuid(),
            NameEn = nameEn,
            NameAr = nameAr,
            DaysFrom = daysFrom,
            DaysTo = daysTo,
            PricePerShipment = pricePerShipment,
            IsSwitchActive = isSwitchActive,
            ImageUrl = imageUrl,
            Description = description,
        };
    }

    public void Update(
        string? nameEn = null,
        string? nameAr = null,
        int? daysFrom = null,
        int? daysTo = null,
        decimal? pricePerShipment = null,
        bool? isSwitchActive = null,
        string? imageUrl = null,
        string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(nameEn)) NameEn = nameEn;
        if (!string.IsNullOrWhiteSpace(nameAr)) NameAr = nameAr;
        if (daysFrom.HasValue) DaysFrom = daysFrom.Value;
        if (daysTo.HasValue) DaysTo = daysTo.Value;
        if (pricePerShipment.HasValue) PricePerShipment = pricePerShipment.Value;
        if (isSwitchActive.HasValue) IsSwitchActive = isSwitchActive.Value;
        if (!string.IsNullOrWhiteSpace(imageUrl)) ImageUrl = imageUrl;
        if (!string.IsNullOrWhiteSpace(description)) Description = description;
    }
}
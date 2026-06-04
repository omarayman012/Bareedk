using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Vehicles.DTO
{
    public class GetVehicleByIdDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Name { get; set; }
        public decimal LoadCapacityFrom { get; set; }
        public decimal LoadCapacityTo { get; set; }
        public decimal PricePerTon { get; set; }
        public decimal? TotalPrice { get; set; }
        public LocalizedDto Currency { get; set; } = default!;
        public LocalizedDto CapacityUnit { get; set; } = default!;
        public string? ImageUrl { get; set; }
        public bool IsPriceCalculationEnabled { get; set; }
        public bool IsActive { get; set; }
    }
}
using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Vehicles.DTO
{
        public class GetAllVehiclesDto
        {
            public Guid Id { get; set; }
        public LocalizedDto Name { get; set; }
        public string? ImageUrl { get; set; }
            public string LoadCapacityFrom { get; set; } = default!;
            public string LoadCapacityTo { get; set; } = default!;
            public string PricePerTon { get; set; } = default!;
            public string TotalPrice { get; set; } = default!;
            public string CreatedBy { get; set; } = default!;
            public DateTime CreatedAt { get; set; }
            public string? UpdatedBy { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public bool IsPriceCalculationEnabled { get; set; }
            public bool IsActive { get; set; }
        }
    }

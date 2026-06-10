using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Vehicles.DTO
{
    public class VehicleExcelExportDto
    {
        public string NameAr { get; set; } = default!;
        public string NameEn { get; set; } = default!;

        public decimal LoadCapacityFrom { get; set; }
        public decimal LoadCapacityTo { get; set; }

        public decimal PricePerTon { get; set; }

        public Currency Currency { get; set; }

        public string? ImageUrl { get; set; }

        public bool IsPriceCalculationEnabled { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    
}
}

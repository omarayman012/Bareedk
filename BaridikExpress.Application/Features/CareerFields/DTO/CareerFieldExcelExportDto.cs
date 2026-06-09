using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.CareerFields.DTO
{
    public class CareerFieldExcelExportDto
    {
        public string NameAr { get; set; } = default!;
        public string NameEn { get; set; } = default!;

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}


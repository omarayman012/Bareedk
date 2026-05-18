using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.LocationGeography.Dto.City
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Name { get; set; } = new();
        public LocalizedNameDto? GovernmentName { get; set; }
        public LocalizedNameDto? CountryName { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

    }
}

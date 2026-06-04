using BaridikExpress.Application.Features.CommanDTO.Localizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Dto
{
    public class PublishingHouseDetailsDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public LocalizedDto Name { get; set; } = new();

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public string? WebsiteLink { get; set; }

        public string? Address { get; set; }

        public string? LogoImage { get; set; }

        // ===== Localized Location =====
        public LocalizedNameDto Country { get; set; }

        public LocalizedNameDto Government { get; set; }

        public LocalizedNameDto? City { get; set; }

        public LocalizedNameDto? Village { get; set; }

        public string? Street { get; set; }

        public string? BuildingNumber { get; set; }

        public string? FloorNumber { get; set; }

        public string? DistinctiveMark { get; set; }

        public string? ZipCode { get; set; }
        public bool Active { get; set; }

        // ===== Audit =====
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
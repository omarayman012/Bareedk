using BaridikExpress.Application.Features.CommanDTO.Localizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Dto
{
    public class PublishingHouseDto
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public LocalizedDto Name { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public string? WebsiteLink { get; set; }

        public string? Address { get; set; }

        public Guid CountryId { get; set; }

        public Guid GovernmentId { get; set; }

        public Guid? CityId { get; set; }

        public Guid? VillageId { get; set; }

        public string? Street { get; set; }

        public string? BuildingNumber { get; set; }

        public string? FloorNumber { get; set; }

        public string? DistinctiveMark { get; set; }

        public string? ZipCode { get; set; }

        public string LogoImage { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Createdby { get; set; }
    }
}
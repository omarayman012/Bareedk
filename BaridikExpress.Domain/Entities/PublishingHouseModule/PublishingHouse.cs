using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.PublishingHouseModule
{

    public class PublishingHouse : BaseEntity
    {
        public Guid Id { get; set; }

        // Required
        public string Code { get; set; } = string.Empty;

        // Required
        public string NameAr { get; set; } = string.Empty;

        public string NameEn { get; set; } = string.Empty;

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public string? WebsiteLink { get; set; }

        public string? Address { get; set; }

        public bool Active { get; set; } = true;

        // Location Relations

        // Required
        public Guid CountryId { get; set; }
        public Country Country { get; set; }

        // Required
        public Guid GovernmentId { get; set; }
        public Government Government { get; set; }

        public Guid? CityId { get; set; }
        public City? City { get; set; }

        public Guid? VillageId { get; set; }
        public Village? Village { get; set; }

        // Address Details

        public string? Street { get; set; }

        public string? BuildingNumber { get; set; }

        public string? FloorNumber { get; set; }

        public string? DistinctiveMark { get; set; }

        public string? ZipCode { get; set; }

        // Required
        public string LogoImage { get; set; } = string.Empty;
    }
}

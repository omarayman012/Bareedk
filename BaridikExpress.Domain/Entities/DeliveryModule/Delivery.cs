using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.DeliveryModule
{
    public class Delivery : BaseEntity
    {

        public Guid Id { get; set; }

        // USER RELATION
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = default!;


        public DateTime DateOfBirth { get; set; }

        // VEHICLE
        public string PlateNo { get; set; } = string.Empty;
        public VehicleType VehType { get; set; }

        // APPROVAL
        public bool IsApproved { get; set; } = false;
        public bool Active { get; set; } = true;
        public DateTime? ApprovedAt { get; set; }
        public DeliveryCreationType CreateType { get; set; }

        // ADDRESS
        public Guid? CountryId { get; set; }
        public Guid? GovernmentId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? VillageId { get; set; }
        public string? Address { get; set; }
        public string? Floor { get; set; }
        public string? Apt { get; set; }

        // OPTIONAL
        public string? Edu { get; set; }

        // FILES
        public string ProfileImg { get; set; }
        public string NidImg { get; set; }
        public string LicImg { get; set; }
        public string VehImg { get; set; }
        public string PoliceCertImg { get; set; } 
        public string PlateImg { get; set; } 
       
        public Country Country { get; set; }
        public Government Government { get; set; }
        public City City { get; set; }
        public Village Village { get; set; }
      

        // TERMS
        public bool TermsAccepted { get; set; } = false;
        public bool PrivacyAccepted { get; set; } = false;
    }
}
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.AuthModules;
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
        public DateTime? ApprovedAt { get; set; }
        public DeliveryCreationType CreateType { get; set; }

        // ADDRESS
        public string? Country { get; set; }
        public string? Gov { get; set; }
        public string? City { get; set; }
        public string? Village { get; set; }
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

        // TERMS
        public bool TermsAccepted { get; set; } = false;
        public bool PrivacyAccepted { get; set; } = false;
    }
}
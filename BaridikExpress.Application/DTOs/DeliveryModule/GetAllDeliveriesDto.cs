using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.DTOs.DeliveryModule
{
    public class GetAllDeliveriesDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        // VEHICLE
        public string PlateNo { get; set; }

        public string VehType { get; set; }

        // APPROVAL
        public bool IsApproved { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public string CreateType { get; set; }

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
        public bool TermsAccepted { get; set; }

        public bool PrivacyAccepted { get; set; }
    }
}

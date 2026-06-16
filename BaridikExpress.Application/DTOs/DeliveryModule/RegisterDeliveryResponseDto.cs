using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.DTOs.DeliveryModule
{
    public class RegisterDeliveryResponseDto
    {
        public string Id { get; set; }

        // BASIC
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }

        // VEHICLE
        public string PlateNo { get; set; } = string.Empty;
        public string VehType { get; set; }
        // APPROVAL
        public bool IsApproved { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public string CreateType { get; set; }

        // ADDRESS
        public LocalizedNameDto? Country { get; set; }
        public LocalizedNameDto? Gov { get; set; }
        public LocalizedNameDto? City { get; set; }
        public LocalizedNameDto? Village { get; set; }
        public string? Address { get; set; }
        public string? Floor { get; set; }
        public string? Apt { get; set; }

        // OPTIONAL
        public string? Edu { get; set; }

        // FILES
        public string ProfileImg { get; set; } = string.Empty;
        public string NidImg { get; set; } = string.Empty;
        public string LicImg { get; set; } = string.Empty;
        public string VehImg { get; set; } = string.Empty;
        public string PoliceCertImg { get; set; } = string.Empty;
        public string PlateImg { get; set; } = string.Empty;

        // TERMS
        public bool TermsAccepted { get; set; }
        public bool PrivacyAccepted { get; set; }
        // AUTH
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
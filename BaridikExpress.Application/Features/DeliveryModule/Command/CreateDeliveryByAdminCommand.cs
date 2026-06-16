using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.AuthDeliveryModule.Command
{
    public class CreateDeliveryByAdminCommand
         : IRequest<Result<RegisterDeliveryResponseDto>>
    {
        public string FullName { get; set; }

        public DateTime DateOfBirth { get; set; } 

        // VEHICLE
        public string PlateNo { get; set; } = string.Empty;

        public VehicleType VehType { get; set; }

        // ADDRESS
        public Guid? Country { get; set; }

        public Guid? Gov { get; set; }

        public Guid? City { get; set; }

        public Guid? Village { get; set; }

        public string? Address { get; set; }

        public string? Floor { get; set; }

        public string? Apt { get; set; }

        // OPTIONAL
        public string? Edu { get; set; }

        // FILES
        public IFormFile ProfileImg { get; set; }

        public IFormFile NidImg { get; set; }

        public IFormFile LicImg { get; set; }

        public IFormFile VehImg { get; set; }

        public IFormFile PoliceCertImg { get; set; }

        public IFormFile PlateImg { get; set; }

        // AUTH
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

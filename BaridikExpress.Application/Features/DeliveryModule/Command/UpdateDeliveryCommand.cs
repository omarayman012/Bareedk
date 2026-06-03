using BaridikExpress.Application.Features.DeliveryModule.DTOs;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Command
{
    public class UpdateDeliveryCommand
         : IRequest<Result<UpdateDeliveryResponseDto>>
    {
        public string Id { get; set; }

        // USER
        public string FullName { get; set; }

        // DELIVERY
        public DateTime DateOfBirth { get; set; }

        // ADDRESS
        public string? Country { get; set; }

        public string? Gov { get; set; }

        public string? City { get; set; }

        public string? Village { get; set; }

        public string? Address { get; set; }

        public string? Floor { get; set; }

        public string? Apt { get; set; }

        // VEHICLE
        public string PlateNo { get; set; }

        public VehicleType VehType { get; set; }

        // FILES
        public IFormFile? NidImg { get; set; }

        public IFormFile? LicImg { get; set; }

        public IFormFile? VehImg { get; set; }

        public IFormFile? PoliceCertImg { get; set; }

        public IFormFile? PlateImg { get; set; }
    }
}

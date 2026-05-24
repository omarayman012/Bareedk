using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Common.Helpers;

namespace BaridikExpress.Application.Features.Customer.Dtos
{
    public class CustomerAddressResponse
    {
        public Guid Id { get; set; }
        public string? AddressType { get; set; }
        public LocalizedEntityDto? Country { get; set; }
        public LocalizedEntityDto? Government { get; set; }
        public LocalizedEntityDto? City { get; set; }
        public LocalizedEntityDto? Village { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
        public string? FloorNumber { get; set; }
        public string? ZipCode { get; set; }
        public string? DistinctiveMark { get; set; }
        public string? Location { get; set; }
        public bool IsDefault { get; set; }
    }
}

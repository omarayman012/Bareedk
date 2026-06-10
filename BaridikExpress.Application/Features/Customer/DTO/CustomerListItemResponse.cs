using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.Customer.Dtos;

namespace BaridikExpress.Application.Features.Customer.DTO
{
    public sealed class CustomerListItemResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Image { get; set; }

        public NationalityDto? Nationality { get; set; }
        public LocalizedEntityDto? CareerField { get; set; }

        public CustomerContactResponse? PrimaryContact { get; set; }
        public CustomerAddressResponse? Address { get; set; }
        public CustomerAccountResponse? Account { get; set; }

        public bool IsActive { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public sealed class NationalityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

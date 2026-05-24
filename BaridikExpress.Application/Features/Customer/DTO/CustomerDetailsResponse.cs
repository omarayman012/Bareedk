using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Common.Helpers;

namespace BaridikExpress.Application.Features.Customer.Dtos
{
    public class CustomerDetailsResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Image { get; set; }

        public List<CustomerContactResponse> Contacts { get; set; } = [];

        public List<CustomerAddressResponse> Addresses { get; set; } = [];

        public CustomerAccountResponse? Account { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Customer.Dtos
{

    public class CustomerContactResponse
    {
        public Guid Id { get; set; }
        public string? PhoneCountryCode { get; set; }
        public string PhoneNumber { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? WhatsAppCountryCode { get; set; }
        public string? WhatsAppNumber { get; set; }
        public bool IsPrimary { get; set; }
    }
}

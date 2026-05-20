using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Customer.Dtos
{
    public class CustomerAccountResponse
    {
        public Guid Id { get; set; }
        public string? TaxRegistrationNumber { get; set; }
        public string? Currency { get; set; }
        public decimal? OpeningBalance { get; set; }
        public DateOnly? OpeningBalanceDate { get; set; }
        public string? Note { get; set; }
    }
}

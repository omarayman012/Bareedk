using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.Customers
{
    public class CustomerAccount : BaseEntity
    {
        public Guid Id { get; private set; }
        public string? TaxRegistrationNumber { get; private set; }
        public Currency? Currency { get; private set; }
        public decimal? OpeningBalance { get; private set; }
        public DateOnly? OpeningBalanceDate { get; private set; }
        public string? Note { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;

        private CustomerAccount() { }

        public static CustomerAccount Create(
            Guid customerId,
            string? taxRegistrationNumber = null,
            Currency? currency = null,
            decimal? openingBalance = null,
            DateOnly? openingBalanceDate = null,
            string? note = null)
        {
            return new CustomerAccount
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                TaxRegistrationNumber = taxRegistrationNumber,
                Currency = currency,
                OpeningBalance = openingBalance,
                OpeningBalanceDate = openingBalanceDate,
                Note = note,
            };
        }
        public void Update(
          string? taxRegistrationNumber = null,
           Currency? currency = null,
           decimal? openingBalance = null,
          DateOnly? openingBalanceDate = null,
           string? note = null)
        {
            if (!string.IsNullOrWhiteSpace(taxRegistrationNumber)) TaxRegistrationNumber = taxRegistrationNumber;
            if (currency is not null) Currency = currency.Value;
            if (openingBalance is not null) OpeningBalance = openingBalance.Value;
            if (openingBalanceDate is not null) OpeningBalanceDate = openingBalanceDate.Value;
            if (!string.IsNullOrWhiteSpace(note)) Note = note;
        }
    }
}

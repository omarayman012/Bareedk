using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Customers
{
    public class CustomerContact : BaseEntity
    {
        public Guid Id { get; private set; }
        public string PhoneCountryCode { get; private set; } = default!;
        public string PhoneNumber { get; private set; } = default!;
        public string Email { get; private set; } = string.Empty;
        public string? WhatsAppCountryCode { get; private set; }
        public string? WhatsAppNumber { get; private set; }
        public bool IsPrimary { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;

        private CustomerContact() { }

        public static CustomerContact Create(
            Guid customerId,
            string phoneCountryCode,
            string phoneNumber,
            string email,
            string? whatsAppCountryCode = null,
            string? whatsAppNumber = null,
            bool isPrimary = false)
        {
            return new CustomerContact
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                PhoneCountryCode = phoneCountryCode,
                PhoneNumber = phoneNumber,
                Email = email,
                WhatsAppCountryCode = whatsAppCountryCode,
                WhatsAppNumber = whatsAppNumber,
                IsPrimary = isPrimary,
            };
        }
        public void Update(
           string? phoneCountryCode = null,
           string? phoneNumber = null,
           string? email = null,
           string? whatsAppCountryCode = null,
           string? whatsAppNumber = null,
           bool? isPrimary = null)
             {
                if (!string.IsNullOrWhiteSpace(phoneCountryCode)) PhoneCountryCode = phoneCountryCode;
                if (!string.IsNullOrWhiteSpace(phoneNumber)) PhoneNumber = phoneNumber;
                if (!string.IsNullOrWhiteSpace(email)) Email = email;
                if (!string.IsNullOrWhiteSpace(whatsAppCountryCode)) WhatsAppCountryCode = whatsAppCountryCode;
                if (!string.IsNullOrWhiteSpace(whatsAppNumber)) WhatsAppNumber = whatsAppNumber;
                if (isPrimary is not null) IsPrimary = isPrimary.Value;
              }
    }  

}

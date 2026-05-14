using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.ValueObjects;

namespace BaridikExpress.Domain.Entities.Customers
{
    public class Customer : BaseEntity
    {
        public Guid Id { get; private set; }
        public string UserId { get; private set; } = string.Empty;
        public Guid CareerFieldId { get; private set; }
        public CareerField CareerField { get; private set; } = default!;
        public LocalizedString Name { get; private set; } = default!;
        public string Email { get; private set; } = string.Empty;
        public string Mobile { get; private set; } = string.Empty;
        public string? WhatsappNumber { get; private set; }
        public string? Nationality { get; private set; }
        public string PasswordHash { get; private set; } = string.Empty;
        public string? CustomerImage { get; private set; }
        public string? TaxRegistrationNumber { get; private set; }
        public decimal OpeningBalance { get; private set; }
        public DateTime? OpeningBalanceDate { get; private set; }
        public string? Notes { get; private set; }

        private Customer() { }
        public Customer(
            string userId,
            Guid careerFieldId,
            string? nameEn,
            string? nameAr,
            string email,
            string mobile,
            string passwordHash,
            string? whatsappNumber,
            string? nationality,
            string? customerImage,
            string? taxRegistrationNumber,
            decimal openingBalance,
            DateTime? openingBalanceDate,
            string? notes)
        {
            Id = Guid.NewGuid();

            SetUserId(userId);

            SetCareerFieldId(careerFieldId);

            SetName(nameEn, nameAr);

            SetEmail(email);

            SetMobile(mobile);

            SetPasswordHash(passwordHash);
            WhatsappNumber = whatsappNumber?.Trim();
            Nationality = nationality?.Trim();
            CustomerImage = customerImage?.Trim();

            TaxRegistrationNumber = taxRegistrationNumber?.Trim();

            SetOpeningBalance(openingBalance);

            OpeningBalanceDate = openingBalanceDate;

            Notes = notes?.Trim();
        }



        public void Update(
            Guid careerFieldId,
            string? nameEn,
            string? nameAr,
            string email,
            string mobile,
            string? whatsappNumber,
            string? nationality,
            string? customerImage,
            string? taxRegistrationNumber,
            decimal openingBalance,
            DateTime? openingBalanceDate,
            string? notes)
        {
            SetCareerFieldId(careerFieldId);

            SetName(nameEn, nameAr);

            SetEmail(email);

            SetMobile(mobile);

            WhatsappNumber = whatsappNumber?.Trim();

            Nationality = nationality?.Trim();

            CustomerImage = customerImage?.Trim();

            TaxRegistrationNumber = taxRegistrationNumber?.Trim();

            SetOpeningBalance(openingBalance);

            OpeningBalanceDate = openingBalanceDate;

            Notes = notes?.Trim();
        }


        private void SetName(
            string? nameEn,
            string? nameAr)
        {
            Name = LocalizedString.Create(nameEn, nameAr);
        }

        private void SetUserId(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException(nameof(userId));

            UserId = userId.Trim();
        }

        private void SetCareerFieldId(Guid careerFieldId)
        {
            if (careerFieldId == Guid.Empty)
                throw new ArgumentException(nameof(careerFieldId));

            CareerFieldId = careerFieldId;
        }

        private void SetEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(nameof(email));

            Email = email.Trim();
        }



        private void SetMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                throw new ArgumentException(nameof(mobile));

            Mobile = mobile.Trim();
        }



        private void SetPasswordHash(string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException(nameof(passwordHash));

            PasswordHash = passwordHash;
        }



        private void SetOpeningBalance(decimal openingBalance)
        {
            if (openingBalance < 0)
                throw new ArgumentException(nameof(openingBalance));

            OpeningBalance = openingBalance;
        }
    }
}
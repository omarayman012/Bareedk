using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Entities.ServiceModules;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.Services
{
    public class TalkService : BaseEntity
    {
        public Guid Id { get; private set; }

        public Guid ServiceBusinessPlanId { get; private set; }
        public ServiceBusinessPlan ServiceBusinessPlan { get; private set; } = null!;

        public ShipmentVolumeRange ShipmentVolumeRange { get; private set; }

        public string FirstName { get; private set; } = null!;
        public string LastName { get; private set; } = null!;

        public Guid CountryId { get; private set; }
        public Country? Country { get; private set; }

        public Guid GovernmentId { get; private set; }
        public Government? Government { get; private set; }

        public Guid ?CityId { get; private set; }
        public City? City { get; private set; }

        public Guid? VillageId { get; private set; }
        public Village? Village { get; private set; }

        public string PostalCode { get; private set; } = null!;
        public string PhoneNumber { get; private set; } = null!;

        public string WorkEmail { get; private set; } = null!;
        public string JobTitle { get; private set; } = null!;
        public string CompanyName { get; private set; } = null!;
        public string CompanyAddress { get; private set; } = null!;
        public string WebsiteUrl { get; private set; } = null!;
        public string AdditionalInformation { get; private set; } = null!;

        public string Status { get; private set; } = "Pending";

        private TalkService() { }

        public static TalkService Create(
            Guid serviceBusinessPlanId,
            ShipmentVolumeRange shipmentVolumeRange,
            string firstName,
            string lastName,
            Guid countryId,
            Guid governmentId,
            Guid cityId,
            Guid? villageId,
            string postalCode,
            string phoneNumber,
            string workEmail,
            string jobTitle,
            string companyName,
            string companyAddress,
            string websiteUrl,
            string additionalInformation)
        {
            return new TalkService
            {
                Id = Guid.NewGuid(),
                ServiceBusinessPlanId = serviceBusinessPlanId,
                ShipmentVolumeRange = shipmentVolumeRange,
                FirstName = firstName,
                LastName = lastName,
                CountryId = countryId,

                GovernmentId = governmentId,
                CityId = cityId,
                VillageId = villageId,
                PostalCode = postalCode,
                PhoneNumber = phoneNumber,
                WorkEmail = workEmail,
                JobTitle = jobTitle,
                CompanyName = companyName,
                CompanyAddress = companyAddress,
                WebsiteUrl = websiteUrl,
                AdditionalInformation = additionalInformation,
                Status = "Pending"
            };
        }

        public void Update(
            Guid? serviceBusinessPlanId,
            ShipmentVolumeRange? shipmentVolumeRange,
            string? firstName,
            string? lastName,
            Guid? countryId,
            Guid? governmentId,
            Guid? cityId,
            Guid? villageId,
            string? postalCode,
            string? phoneNumber,
            string? workEmail,
            string? jobTitle,
            string? companyName,
            string? companyAddress,
            string? websiteUrl,
            string? additionalInformation,
            string? status)
        {
            if (serviceBusinessPlanId is not null && serviceBusinessPlanId != Guid.Empty) ServiceBusinessPlanId = serviceBusinessPlanId.Value;
            if (shipmentVolumeRange is not null) ShipmentVolumeRange = shipmentVolumeRange.Value;
            if (firstName is not null) FirstName = firstName;
            if (lastName is not null) LastName = lastName;
            if (countryId is not null && countryId != Guid.Empty) CountryId = countryId.Value;
            if (governmentId is not null && governmentId != Guid.Empty) GovernmentId = governmentId.Value;
            if (cityId is not null && cityId != Guid.Empty) CityId = cityId.Value;
            if (villageId is not null) VillageId = villageId;
            if (postalCode is not null) PostalCode = postalCode;
            if (phoneNumber is not null) PhoneNumber = phoneNumber;
            if (workEmail is not null) WorkEmail = workEmail;
            if (jobTitle is not null) JobTitle = jobTitle;
            if (companyName is not null) CompanyName = companyName;
            if (companyAddress is not null) CompanyAddress = companyAddress;
            if (websiteUrl is not null) WebsiteUrl = websiteUrl;
            if (additionalInformation is not null) AdditionalInformation = additionalInformation;
            if (status is not null) Status = status;
        }
    }
}
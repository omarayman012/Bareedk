using System;
using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.Addresses
{
    public class ClientAddress : BaseEntity
    {
        public Guid Id { get; private set; }
        public AddressType AddressType { get; private set; }

        #region Foreign Keys Location
        public Guid CountryId { get; private set; }
        public Country Country { get; private set; } = default!;
        public Guid GovernmentId { get; private set; }       
        public Government Government { get; private set; } = default!;
        public Guid CityId { get; private set; }
        public City City { get; private set; } = default!;
        public Guid? VillageId { get; private set; }
        public Village? Village { get; private set; }
        #endregion

        public string Street { get; private set; } = default!;
        public string BuildingNumber { get; private set; } = default!;
        public string? FloorNumber { get; private set; }
        public string? DistinctiveMark { get; private set; }
        public string? ZipCode { get; private set; }
        public bool IsDefault { get; private set; }

        #region Recipient Information
        public string RecipientName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string PhoneNumber { get; private set; } = default!;
        #endregion

        #region Address Information
        public string? FlatNumber { get; private set; }
        public string? AddressTitle { get; private set; }
        #endregion

        #region Map Information
        public decimal? Latitude { get; private set; }
        public decimal? Longitude { get; private set; }
        public string? Location { get; private set; }
        #endregion
        
        public string UserId { get; private set; }
        public User User { get; private set; } = default!;

        private ClientAddress() { }
        public static ClientAddress CreateAddress(
    string userId,
    AddressType ?addressType,
    string recipientName,
    string email,
    string phoneNumber,
    string? addressTitle,
    string? flatNumber,
    decimal? latitude,
    decimal? longitude,
    Guid countryId,
    Guid governmentId,
    Guid cityId,
    Guid? villageId = null,
    string? street = null,
    string? buildingNumber = null,
    string? floorNumber = null,
    string? distinctiveMark = null,
    string? zipCode = null,
    string? location = null,
    bool isDefault = false)
        {
            return new ClientAddress
            {
                Id = Guid.NewGuid(),

                UserId = userId,
                AddressType = addressType ?? AddressType.Home,
                CountryId = countryId,
                GovernmentId = governmentId,
                CityId = cityId,
                VillageId = villageId,
                Street = street ?? string.Empty,
                BuildingNumber = buildingNumber ?? string.Empty,
                FloorNumber = floorNumber,
                DistinctiveMark = distinctiveMark,
                ZipCode = zipCode,

                RecipientName = recipientName,
                Email = email,
                PhoneNumber = phoneNumber,

                FlatNumber = flatNumber,
                AddressTitle = addressTitle,

                Latitude = latitude,
                Longitude = longitude,
                Location = location,

                IsDefault = isDefault
            };
        }
        public void UpdateAddress(
    AddressType? addressType = null,
    Guid? countryId = null,
    Guid? governmentId = null,
    Guid? cityId = null,
    Guid? villageId = null,

    string? street = null,
    string? buildingNumber = null,
    string? floorNumber = null,
    string? flatNumber = null,
    string? distinctiveMark = null,
    string? zipCode = null,

    string? recipientName = null,
    string? addressTitle = null,
    string? email = null,
    string? phoneNumber = null,

    decimal? latitude = null,
    decimal? longitude = null,
    string? location = null,

    bool? isDefault = null)
        {
            if (addressType is not null)
                AddressType = addressType.Value;

            if (countryId is not null)
                CountryId = countryId.Value;

            if (governmentId is not null)
                GovernmentId = governmentId.Value;

            if (cityId is not null)
                CityId = cityId.Value;

            if (villageId is not null)
                VillageId = villageId.Value;

            if (!string.IsNullOrWhiteSpace(street))
                Street = street;

            if (!string.IsNullOrWhiteSpace(buildingNumber))
                BuildingNumber = buildingNumber;

            if (!string.IsNullOrWhiteSpace(floorNumber))
                FloorNumber = floorNumber;

            if (!string.IsNullOrWhiteSpace(flatNumber))
                FlatNumber = flatNumber;

            if (!string.IsNullOrWhiteSpace(distinctiveMark))
                DistinctiveMark = distinctiveMark;

            if (!string.IsNullOrWhiteSpace(zipCode))
                ZipCode = zipCode;

            if (!string.IsNullOrWhiteSpace(recipientName))
                RecipientName = recipientName;

            if (!string.IsNullOrWhiteSpace(email))
                Email = email;

            if (!string.IsNullOrWhiteSpace(phoneNumber))
                PhoneNumber = phoneNumber;

            if (!string.IsNullOrWhiteSpace(addressTitle))
                AddressTitle = addressTitle;

            if (latitude.HasValue)
                Latitude = latitude.Value;

            if (longitude.HasValue)
                Longitude = longitude.Value;

            if (!string.IsNullOrWhiteSpace(location))
                Location = location;

            if (isDefault.HasValue)
                IsDefault = isDefault.Value;
        }


    }
}

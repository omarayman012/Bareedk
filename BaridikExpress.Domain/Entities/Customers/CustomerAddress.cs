using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Location;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.Customers
{
    public class CustomerAddress : BaseEntity
    {
        public Guid Id { get; private set; }
        public AddressType? AddressType { get; private set; }

        public Guid? CountryId { get; private set; }       
        public Country? Country { get; private set; }

        public Guid? GovernmentId { get; private set; }       
        public Government? Government { get; private set; }

        public Guid? CityId { get; private set; }            
        public City? City { get; private set; }

        public Guid? VillageId { get; private set; }         
        public Village? Village { get; private set; }

        public string Street { get; private set; } = default!;
        public string BuildingNumber { get; private set; } = default!;
        public string? FloorNumber { get; private set; }
        public string? DistinctiveMark { get; private set; }
        public string? ZipCode { get; private set; }
        public bool IsDefault { get; private set; }

        #region Recipient Information

        public string? RecipientName { get; private set; }

        public string? Email { get; private set; }

        public string? PhoneNumber { get; private set; }

        #endregion

        #region Address Information
        public string? ApartmentNumber { get; private set; }
        public string? AddressTitle { get; private set; }

        #endregion

        #region Map Information

        public decimal? Latitude { get; private set; }
        public decimal? Longitude { get; private set; }
        public string? Location { get; private set; }
        #endregion

        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;
        private CustomerAddress() { }



        #region Address Creation and Update Methods For Dashboard not include Recipient Information + Map Information
        public static CustomerAddress Create(
            Guid customerId,
            AddressType ?addressType,
            Guid? countryId = null,
            Guid? governmentId = null,
            Guid? cityId = null,
            Guid? villageId = null,
            string? street = null,
            string? buildingNumber = null,
            string? floorNumber = null,
            string? distinctiveMark = null,
            string? zipCode = null,
            string? location = null,
            bool isDefault = false)
        {
            return new CustomerAddress
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                AddressType = addressType,
                CountryId = countryId,
                GovernmentId = governmentId,
                CityId = cityId,
                VillageId = villageId,
                Street = street ?? string.Empty,
                BuildingNumber = buildingNumber ?? string.Empty,
                FloorNumber = floorNumber,
                DistinctiveMark = distinctiveMark,
                ZipCode = zipCode,
                Location = location,
                IsDefault = isDefault,


                //add in Screen mobile 
                RecipientName = null,
                AddressTitle = null,
                Email = null,
                PhoneNumber = null,
                ApartmentNumber = null,
                Latitude = null,
                Longitude = null,
            };
        }

        public void Update(
            AddressType? addressType = null,
            Guid? countryId = null,
            Guid? governmentId = null,
            Guid? cityId = null,
            Guid? villageId = null,
            string? street = null,
            string? buildingNumber = null,
            string? floorNumber = null,
            string? distinctiveMark = null,
            string? zipCode = null,
            string? location = null,
            bool? isDefault = null)
        {
            if (addressType is not null) AddressType = addressType.Value;
            if (countryId is not null) CountryId = countryId.Value;
            if (governmentId is not null) GovernmentId = governmentId.Value;
            if (cityId is not null) CityId = cityId.Value;
            if (villageId is not null) VillageId = villageId.Value;
            if (!string.IsNullOrWhiteSpace(street)) Street = street;
            if (!string.IsNullOrWhiteSpace(buildingNumber)) BuildingNumber = buildingNumber;
            if (!string.IsNullOrWhiteSpace(floorNumber)) FloorNumber = floorNumber;
            if (!string.IsNullOrWhiteSpace(distinctiveMark)) DistinctiveMark = distinctiveMark;
            if (!string.IsNullOrWhiteSpace(zipCode)) ZipCode = zipCode;
            if (!string.IsNullOrWhiteSpace(location)) Location = location;
            if (isDefault is not null) IsDefault = isDefault.Value;
        }
        #endregion


        #region Recipient Information + Address Information + Map Information in Screen Mobile
        public static CustomerAddress CreateAddress(
            Guid customerId,
            AddressType? addressType,
            string? recipientName,
            string? email,
            string? phoneNumber,
            string? addressTitle,
            string? apartmentNumber,
            decimal? latitude,
            decimal? longitude,
            Guid? countryId = null,
            Guid? governmentId = null,
            Guid? cityId = null,
            Guid? villageId = null,
            string? street = null,
            string? buildingNumber = null,
            string? floorNumber = null,
            string? distinctiveMark = null,
            string? zipCode = null,
            string? location = null,
            bool isDefault = false)
        {
            return new CustomerAddress
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,

                AddressType = addressType,

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
                ApartmentNumber = apartmentNumber,
                AddressTitle = addressTitle,
                Latitude = latitude,
                Longitude = longitude,
                Location = location,

                IsDefault = isDefault
            };
        }


        public void UpdateCustomerAddress(
            AddressType? addressType = null,
            Guid? countryId = null,
            Guid? governmentId = null,
            Guid? cityId = null,
            Guid? villageId = null,

            string? street = null,
            string? buildingNumber = null,
            string? floorNumber = null,
            string? apartmentNumber = null,
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

            if (!string.IsNullOrWhiteSpace(apartmentNumber))
                ApartmentNumber = apartmentNumber;

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
        #endregion



    }
}
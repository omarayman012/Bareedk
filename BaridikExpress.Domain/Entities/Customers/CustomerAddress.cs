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
        public string? Location { get; private set; }
        public bool IsDefault { get; private set; }
        public Guid CustomerId { get; private set; }
        public Customer Customer { get; private set; } = default!;

        private CustomerAddress() { }

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
    }
}
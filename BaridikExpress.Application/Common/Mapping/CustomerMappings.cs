using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.Customer.Dtos;
using BaridikExpress.Domain.Entities.Customers;
using System.Linq.Expressions;

namespace BaridikExpress.Application.Common.Mapping
{
    public static class CustomerMappings
    {
        public static Expression<Func<CustomerContact, CustomerContactResponse>> ContactProjection =>
            contact => new CustomerContactResponse
            {
                Id = contact.Id,
                PhoneCountryCode = contact.PhoneCountryCode,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                WhatsAppCountryCode = contact.WhatsAppCountryCode,
                WhatsAppNumber = contact.WhatsAppNumber,
                IsPrimary = contact.IsPrimary
            };

        public static Expression<Func<CustomerAddress, CustomerAddressResponse>> AddressProjection =>
            a => new CustomerAddressResponse
            {
                Id = a.Id,
                AddressType = a.AddressType.ToString(),
                IsDefault = a.IsDefault,
                Street = a.Street,
                BuildingNumber = a.BuildingNumber,
                FloorNumber = a.FloorNumber,
                ZipCode = a.ZipCode,
                DistinctiveMark = a.DistinctiveMark,
                Location = a.Location,

                Country = a.Country == null ? null : new LocalizedEntityDto
                {
                    Id = a.Country.CountryId,
                    Name = new LocalizeLang
                    {
                        AR = a.Country.CountryNameAr,
                        EN = a.Country.CountryNameEn
                    }
                },

                Government = a.Government == null ? null : new LocalizedEntityDto
                {
                    Id = a.Government.GovernmentId,
                    Name = new LocalizeLang
                    {
                        AR = a.Government.GovernmentNameAr,
                        EN = a.Government.GovernmentNameEn
                    }
                },

                City = a.City == null ? null : new LocalizedEntityDto
                {
                    Id = a.City.CityId,
                    Name = new LocalizeLang
                    {
                        AR = a.City.CityNameAr,
                        EN = a.City.CityNameEn
                    }
                },

                Village = a.Village == null ? null : new LocalizedEntityDto
                {
                    Id = a.Village.VillageId,
                    Name = new LocalizeLang
                    {
                        AR = a.Village.VillageNameAr,
                        EN = a.Village.VillageNameEn
                    }
                }
            };
    }
}

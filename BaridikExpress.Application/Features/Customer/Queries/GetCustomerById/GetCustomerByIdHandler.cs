using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.Customer.Dtos;

namespace BaridikExpress.Application.Features.Customer.Queries.GetCustomerById
{
    public class GetCustomerByIdHandler(IApplicationDbContext context, IStringLocalizer localizer)
                : IRequestHandler<GetCustomerByIdQuery, Result<CustomerDetailsResponse>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IStringLocalizer _localizer = localizer;

        public async Task<Result<CustomerDetailsResponse>> Handle(
              GetCustomerByIdQuery request,
             CancellationToken cancellationToken)
        {
            var response = await _context.Customers
                .AsNoTracking()
                .Where(c => c.Id == request.Id)
                .Select(c => new CustomerDetailsResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Image = c.ImageUrl,

                    Contacts = c.Contacts.Select(contact => new CustomerContactResponse
                    {
                        Id = contact.Id,
                        PhoneCountryCode = contact.PhoneCountryCode,
                        PhoneNumber = contact.PhoneNumber,
                        Email = contact.Email,
                        WhatsAppCountryCode = contact.WhatsAppCountryCode,
                        WhatsAppNumber = contact.WhatsAppNumber,
                        IsPrimary = contact.IsPrimary
                    }).ToList(),

                    Addresses = c.Addresses.Select(a => new CustomerAddressResponse
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
                    }).ToList(),

                    Account = c.Account == null ? null : new CustomerAccountResponse
                    {
                        Id = c.Account.Id,
                        TaxRegistrationNumber = c.Account.TaxRegistrationNumber,
                        Currency = c.Account.Currency.ToString(),
                        OpeningBalance = c.Account.OpeningBalance,
                        OpeningBalanceDate = c.Account.OpeningBalanceDate,
                        Note = c.Account.Note
                    },

                    CreatedBy = c.CreatedBy != null ? c.CreatedBy.FullName : null,
                    CreatedAt = c.CreatedAt,
                    UpdatedBy = c.UpdatedBy != null ? c.UpdatedBy.FullName : null,
                    UpdatedAt = c.UpdatedAt,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (response is null)
                return Result<CustomerDetailsResponse>
                    .Failure(_localizer["CustomerNotFound"], 404);

            return Result<CustomerDetailsResponse>
                .Success(response, _localizer["CustomerRetrievedSuccessfully"]);
        }
    }
}
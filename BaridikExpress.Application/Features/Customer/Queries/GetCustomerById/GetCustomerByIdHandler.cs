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
            var customer = await _context.Customers
                .AsNoTracking()
                .Include(c => c.Contacts)
                .Include(c => c.Addresses)
                    .ThenInclude(a => a.Country)
                .Include(c => c.Addresses)
                    .ThenInclude(a => a.Government)
                .Include(c => c.Addresses)
                    .ThenInclude(a => a.City)
                .Include(c => c.Addresses)
                    .ThenInclude(a => a.Village)
                .Include(c => c.Account)
                .Include(c => c.CreatedBy)
                .Include(c => c.UpdatedBy)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (customer is null)
                return Result<CustomerDetailsResponse>
                    .Failure(_localizer["CustomerNotFound"], 404);

            var response = new CustomerDetailsResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Image = customer.ImageUrl,

                Contacts = customer.Contacts.Select(c => new CustomerContactResponse
                {
                    Id = c.Id,
                    PhoneCountryCode = c.PhoneCountryCode,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email,
                    WhatsAppCountryCode = c.WhatsAppCountryCode,
                    WhatsAppNumber = c.WhatsAppNumber,
                    IsPrimary = c.IsPrimary
                }).ToList(),

                Addresses = customer.Addresses.Select(a => new CustomerAddressResponse
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

                    Country = a.Country is not null ? new LocalizedEntityDto
                    {
                        Id = a.Country.CountryId,
                        Name = new LocalizeLang
                        {
                            AR = a.Country.CountryNameAr,
                            EN = a.Country.CountryNameEn
                        }
                    } : null,

                    Government = a.Government is not null ? new LocalizedEntityDto
                    {
                        Id = a.Government.GovernmentId,
                        Name = new LocalizeLang
                        {
                            AR = a.Government.GovernmentNameAr,
                            EN = a.Government.GovernmentNameEn
                        }
                    } : null,

                    City = a.City is not null ? new LocalizedEntityDto
                    {
                        Id = a.City.CityId,
                        Name = new LocalizeLang
                        {
                            AR = a.City.CityNameAr,
                            EN = a.City.CityNameEn
                        }
                    } : null,

                    Village = a.Village is not null ? new LocalizedEntityDto
                    {
                        Id = a.Village.VillageId,
                        Name = new LocalizeLang
                        {
                            AR = a.Village.VillageNameAr,
                            EN = a.Village.VillageNameEn
                        }
                    } : null,

                }).ToList(),

                Account = customer.Account is not null ? new CustomerAccountResponse
                {
                    Id = customer.Account.Id,
                    TaxRegistrationNumber = customer.Account.TaxRegistrationNumber,
                    Currency = customer.Account.Currency.ToString(),
                    OpeningBalance = customer.Account.OpeningBalance,
                    OpeningBalanceDate = customer.Account.OpeningBalanceDate,
                    Note = customer.Account.Note
                } : null,

                CreatedBy = customer.CreatedBy?.FullName,
                CreatedAt = customer.CreatedAt,
                UpdatedBy = customer.UpdatedBy?.FullName,
                UpdatedAt = customer.UpdatedAt,
            };

            return Result<CustomerDetailsResponse>
                .Success(response, _localizer["CustomerRetrievedSuccessfully"]);
        }
    }
}
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.Customer.DTO;
using BaridikExpress.Application.Features.Customer.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Customer.Queries.GetAllCustomers;

public sealed class GetAllCustomersQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllCustomersQuery, Result<PaginatedList<CustomerListItemResponse>>>
{
    public async Task<Result<PaginatedList<CustomerListItemResponse>>> Handle(
        GetAllCustomersQuery request,
        CancellationToken cancellationToken)
    {
        #region Build Query
        var query = db.Customers
            .AsNoTracking()
            .AsQueryable();
        #endregion

        #region Filters
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(c => c.Name.Contains(request.Name));
        if (request.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.IsActive.Value);
        if (!string.IsNullOrWhiteSpace(request.CreatedById))
            query = query.Where(c => c.CreatedById == request.CreatedById);
        if (request.FromDate.HasValue)
            query = query.Where(c => c.CreatedAt >= request.FromDate.Value);
        if (request.ToDate.HasValue)
            query = query.Where(c => c.CreatedAt <= request.ToDate.Value);
        #endregion

        #region Projection
        var projected = query
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CustomerListItemResponse
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.ImageUrl,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy != null ? c.CreatedBy.FullName : null,
                UpdatedBy = c.UpdatedBy != null ? c.UpdatedBy.FullName : null,
                UpdatedAt = c.UpdatedAt,

                Nationality = c.Nationality == null ? null : new NationalityDto
                {
                    Id = c.Nationality.Id,
                    Name = c.Nationality.Name
                },

                CareerField = c.CareerField == null ? null : new LocalizedEntityDto
                {
                    Id = c.CareerField.Id,
                    Name = new LocalizeLang
                    {
                        AR = c.CareerField.Name.Ar,
                        EN = c.CareerField.Name.En
                    }
                },

                PrimaryContact = c.Contacts
                    .Where(x => x.IsPrimary)
                    .Select(x => new CustomerContactResponse
                    {
                        Id = x.Id,
                        PhoneCountryCode = x.PhoneCountryCode,
                        PhoneNumber = x.PhoneNumber,
                        Email = x.Email,
                        WhatsAppCountryCode = x.WhatsAppCountryCode,
                        WhatsAppNumber = x.WhatsAppNumber,
                        IsPrimary = x.IsPrimary
                    })
                    .FirstOrDefault(),

                Address = c.Addresses
                    .Where(a => a.IsDefault)
                    .Select(a => new CustomerAddressResponse
                    {
                        Id = a.Id,
                        AddressType = a.AddressType.ToString(),
                        Street = a.Street,
                        BuildingNumber = a.BuildingNumber,
                        FloorNumber = a.FloorNumber,
                        ZipCode = a.ZipCode,
                        DistinctiveMark = a.DistinctiveMark,
                        Location = a.Location,
                        IsDefault = a.IsDefault,
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
                    })
                    .FirstOrDefault(),

                Account = c.Account == null ? null : new CustomerAccountResponse
                {
                    Id = c.Account.Id,
                    TaxRegistrationNumber = c.Account.TaxRegistrationNumber,
                    Currency = c.Account.Currency.ToString(),
                    OpeningBalance = c.Account.OpeningBalance,
                    OpeningBalanceDate = c.Account.OpeningBalanceDate,
                    Note = c.Account.Note
                },
            });
        #endregion

        #region Paginate
        var result = await PaginatedList<CustomerListItemResponse>
            .CreateAsync(projected, request.PageNumber, request.PageSize);
        #endregion

        return Result<PaginatedList<CustomerListItemResponse>>
            .Success(result, localizer["CustomersRetrievedSuccessfully"]);
    }
}
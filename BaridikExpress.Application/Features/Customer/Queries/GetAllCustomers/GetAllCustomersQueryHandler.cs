using BaridikExpress.Application.Features.Customer.DTO;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.Customer.Queries.GetAllCustomers;

public sealed class GetAllCustomersQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllCustomersQuery, Result<PaginatedList<CustomerListItemResponse>>>
{
    #region Handle

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
                
                PrimaryEmail = c.Contacts
                    .Where(x => x.IsPrimary)
                    .Select(x => x.Email)
                    .FirstOrDefault(),

                PrimaryPhone = c.Contacts
                    .Where(x => x.IsPrimary)
                    .Select(x => x.PhoneCountryCode + x.PhoneNumber)
                    .FirstOrDefault(),

                PrimaryWhatsApp = c.Contacts
                    .Where(x => x.IsPrimary)
                    .Select(x => x.WhatsAppCountryCode + x.WhatsAppNumber)
                    .FirstOrDefault(),
                Location = c.Addresses
                 .Select(a => a.Location)
                 .FirstOrDefault(),
            });

        #endregion

        #region Paginate

        var result = await PaginatedList<CustomerListItemResponse>
            .CreateAsync(projected, request.PageNumber, request.PageSize);

        #endregion

        return Result<PaginatedList<CustomerListItemResponse>>
           .Success(result, localizer["CustomersRetrievedSuccessfully"]);
    }

    #endregion
}
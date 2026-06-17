using BaridikExpress.Application.Common.Mapping;
using BaridikExpress.Application.Features.CareerFields.DTO;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.OurPlans.DTO;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.OurPlans;

namespace BaridikExpress.Application.Features.OurPlans.Queries.GetPlanById;

public sealed class GetPlanByIdQueryHandler(
    IGenericRepository<Plan> repo,
    IStringLocalizer localizer)
    : IRequestHandler<
        GetPlanByIdQuery,
        Result<GetPlanByIdDto>>
{
    private readonly IGenericRepository<Plan> _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<GetPlanByIdDto>> Handle(
        GetPlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        var plan = await _repo
            .Query()
            .Include(x => x.CreatedBy)
            .Include(x => x.UpdatedBy)
            .Include(x => x.Customers)
             .ThenInclude(x => x.Addresses)
            .Include(x => x.Customers)
                .ThenInclude(x => x.Contacts)
            .Where(x => x.Id == request.Id)
            .Select(x => new GetPlanByIdDto
            {
                Id = x.Id,

                Name = new LocalizedDto
                {
                    AR = x.NameAr,
                    EN = x.NameEn
                },

                Type = x.Type,

                Description = new LocalizedDto
                {
                    AR = x.DescriptionAr,
                    EN = x.DescriptionEn
                },
                Features= new LocalizedListDto
                {
                    AR = x.FeaturesAr,
                    EN = x.FeaturesEn 
                },

                IsActive = x.IsActive,

                CreatedBy = x.CreatedBy != null
                    ? x.CreatedBy.FullName
                    : "",

                CreatedAt = x.CreatedAt,

                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : "",

                UpdatedAt = x.UpdatedAt,

                CustomersCount = x.Customers.Count,

                Customers = x.Customers
                    .Select(c => new CareerFieldCustomerDto
                    {
                        Id = c.Id,
                        Name = c.Name,

                        Addresses = c.Addresses
                            .AsQueryable()
                            .Select(CustomerMappings.AddressProjection)
                            .ToList(),

                        Contacts = c.Contacts
                            .AsQueryable()
                            .Select(CustomerMappings.ContactProjection)
                            .ToList(),

                        // TODO: Replace when Shipment relation is implemented
                        TotalShipments = 0,

                        // TODO: Replace when Payments relation is implemented
                        TotalSpent = 0,

                        IsActive = c.IsActive
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (plan is null)
        {
            return Result<GetPlanByIdDto>.Failure(
                _localizer["PlanNotFound"],
                404);
        }

        return Result<GetPlanByIdDto>.Success(
            plan,
            _localizer["OperationCompletedSuccessfully"]);
    }
}
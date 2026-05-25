using BaridikExpress.Application.Common.Mapping;
using BaridikExpress.Application.Features.CareerFields.DTO;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Interfaces.IRepository;
using BaridikExpress.Domain.Entities.CareerFields;

namespace BaridikExpress.Application.Features.CareerFields.Queries.GetCareerFieldById;
//TODO: Refactor mapping after implementing Customer
public class GetCareerFieldByIdQueryHandler(
    IGenericRepository<CareerField> repo,
    IStringLocalizer localizer
) : IRequestHandler<
        GetCareerFieldByIdQuery,
        Result<GetCareerFieldByIdDto>
    >
{
    private readonly IGenericRepository<CareerField> _repo = repo;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<GetCareerFieldByIdDto>> Handle(
        GetCareerFieldByIdQuery request,
        CancellationToken cancellationToken)
    {
        var careerField = await _repo
            .Query()
            .Where(x => x.Id == request.Id)
            .Select(x => new GetCareerFieldByIdDto
            {
                Id = x.Id,
                Name= new LocalizedDto
                {
                    AR = x.Name.Ar,
                    EN = x.Name.En
                },
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy!.FullName,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : null,
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
                        // TotalShipments = c.Shipments.Count  ,
                        TotalShipments = 0,
                        //TotalSpent = c.Payments.Sum(p => p.Amount),
                        TotalSpent = 0,
                        IsActive = c.IsActive
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (careerField is null)
        {
            return Result<GetCareerFieldByIdDto>.Failure(
                _localizer["CareerFieldNotFound"],
                404
            );
        }

        return Result<GetCareerFieldByIdDto>.Success(
            careerField,
            _localizer["OperationCompletedSuccessfully"]
        );
    }
}
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.Government;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Government.GetById;

public class GetGovernmentByIdQueryHandler(
    IApplicationDbContext applicationDb,
    IStringLocalizer localizer)
    : IRequestHandler<GetGovernmentByIdQuery,
        Result<GovernmentDto>>
{
    private readonly IApplicationDbContext _applicationDb = applicationDb;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<GovernmentDto>> Handle(
        GetGovernmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var government = await _applicationDb.Governments
            .AsNoTracking()
            .Include(x => x.Country)
            .Where(x => x.GovernmentId == request.Id)
            .Select(x => new GovernmentDto
            {
                Id = x.GovernmentId,

                Name = new LocalizedDto
                {
                    AR = x.GovernmentNameAr,
                    EN = x.GovernmentNameEn
                },

                Country = new LocalizedNameDto
                {
                    Id = x.Country!.CountryId,
                    AR = x.Country.CountryNameAr,
                    EN = x.Country.CountryNameEn
                },

                CreatedBy = x.CreatedBy != null
                    ? x.CreatedBy.FullName
                    : " ",

                CreatedAt = x.CreatedAt,

                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : x.UpdatedById,

                UpdatedAt = x.UpdatedAt,

                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (government is null)
        {
            return Result<GovernmentDto>
                .Failure(_localizer["GovernmentNotFound"]);
        }

        return Result<GovernmentDto>
            .Success(
                government,
                _localizer["GovernmentRetrievedSuccessfully"]);
    }
}
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.Country;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetById;

public class GetCountryByIdQueryHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer) : IRequestHandler<GetCountryByIdQuery, Result<GetCountryResponse>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<GetCountryResponse>> Handle(
        GetCountryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var country = await _application.Countries
            .AsNoTracking()
            .Where(x => x.CountryId == request.Id)
            .Select(x => new GetCountryResponse
            {
                Id = x.CountryId,
                Name = new LocalizedDto
                {
                    AR = x.CountryNameAr,
                    EN = x.CountryNameEn
                },
                PhoneCode = x.PhoneCode,
                PostalCode = x.PostalCode,
                CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : x.CreatedById,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FullName : x.UpdatedById,
                UpdatedAt = x.UpdatedAt,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (country is null)
            return Result<GetCountryResponse>.Failure(_localizer["CountryNotFound"], 404);

        return Result<GetCountryResponse>.Success(country, _localizer["CountryRetrievedSuccessfully"]);
    }
}
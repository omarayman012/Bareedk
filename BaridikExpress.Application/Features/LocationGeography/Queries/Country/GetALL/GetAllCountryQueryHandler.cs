using AutoMapper;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.Country;

namespace BaridikExpress.Application.Features.LocationGeography.Queries.Country.GetALL;

public class GetAllCountryQueryHandler(
    IApplicationDbContext applicationDb,
    IStringLocalizer localizer,
    IMapper mapper) : IRequestHandler<GetAllCountryQuery, Result<PaginatedList<GetCountryResponse>>>
{
    private readonly IApplicationDbContext _applicationDb = applicationDb;
    private readonly IStringLocalizer _localizer = localizer;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<PaginatedList<GetCountryResponse>>> Handle(
        GetAllCountryQuery request,
        CancellationToken cancellationToken)
    {
        var query = _applicationDb.Countries
            .AsNoTracking()
            .Select(x => new GetCountryResponse
            {
                Id = x.CountryId,
                Name = new LocalizedDto
                {
                    AR = x.CountryNameAr,
                    EN = x.CountryNameEn
                },
                CreatedBy = x.CreatedBy != null ? x.CreatedBy.UserName : x.CreatedById,
                CreatedAt = x.CreatedAt,
                UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.UserName : x.UpdatedById,
                UpdatedAt = x.UpdatedAt,
                IsActive= x.IsActive
            });

        var paginatedCountries = await PaginatedList<GetCountryResponse>
            .CreateAsync(query, request.PageNumber, request.PageSize);

        return Result<PaginatedList<GetCountryResponse>>.Success(paginatedCountries, _localizer["CountriesRetrievedSuccessfully"]);
    }
}
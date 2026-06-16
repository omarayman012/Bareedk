using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.LocationGeography.Dto.Country;
using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;

public class CreateCountryCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer) : IRequestHandler<CreateCountryCommand, Result<CreateCountryResponse>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;

    public async Task<Result<CreateCountryResponse>> Handle(
        CreateCountryCommand request,
        CancellationToken cancellationToken)
    {
        var (nameAr, nameEn) = NormalizeHelper.Normalize(request.NameAr, request.NameEn);

        var exists = await _application.Countries
            .AnyAsync(x => x.CountryNameEn == nameEn ||
                           x.CountryNameAr == nameAr ||
                           x.PhoneCode == request.PhoneCode,
                      cancellationToken);

        if (exists)
            return Result<CreateCountryResponse>.Failure(_localizer["CountryAlreadyExists"]);

        var country = new Domain.Entities.Location.Country
        {
            CountryNameAr = nameAr,
            CountryNameEn = nameEn,
            PhoneCode = request.PhoneCode,
            PostalCode = request.PostalCode 
        };

        await _application.Countries.AddAsync(country, cancellationToken);
        await _application.SaveChangesAsync(cancellationToken);

        var response = new CreateCountryResponse
        {
            Id = country.Id,
            Name = new LocalizedDto
            {
                EN = country.CountryNameEn,
                AR = country.CountryNameAr
            },
            PhoneCode = country.PhoneCode,
            PostalCode = country.PostalCode,
            CreatedBy = country.CreatedBy?.FullName ?? string.Empty,
            CreatedAt = country.CreatedAt,
            UpdatedBy = country.UpdatedBy?.FullName ?? string.Empty,
            UpdatedAt = country.UpdatedAt
        };

        return Result<CreateCountryResponse>.Success(response, _localizer["CountryCreatedSuccessfully"]);
    }
}
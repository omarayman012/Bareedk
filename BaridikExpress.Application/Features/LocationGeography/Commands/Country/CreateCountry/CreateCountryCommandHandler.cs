using AutoMapper;
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.LocationGeography.Dto.Country;
using BaridikExpress.Application.Interfaces.IRepository;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.CreateCountry;

public class CreateCountryCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer,
    IMapper mapper) : IRequestHandler<CreateCountryCommand, Result<CreateCountryResponse>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;
    private readonly IMapper _mapper = mapper;

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

        var country = _mapper.Map<Domain.Entities.Location.Country>(request);


        await _application.Countries.AddAsync(country, cancellationToken);
        await _application.SaveChangesAsync(cancellationToken);

        var response = _mapper.Map<CreateCountryResponse>(country);

        return Result<CreateCountryResponse>.Success(response, _localizer["CountryCreatedSuccessfully"]);
    }
}
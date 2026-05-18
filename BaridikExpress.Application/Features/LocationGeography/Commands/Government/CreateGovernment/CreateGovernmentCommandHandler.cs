using AutoMapper;
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.LocationGeography.Dto.Government;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.CreateGovernment;

public class CreateGovernmentCommandHandler(
    IApplicationDbContext application,
    IStringLocalizer localizer,
    IMapper mapper)
    : IRequestHandler<CreateGovernmentCommand, Result<GovernmentDto>>
{
    private readonly IApplicationDbContext _application = application;
    private readonly IStringLocalizer _localizer = localizer;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<GovernmentDto>> Handle(
        CreateGovernmentCommand request,
        CancellationToken cancellationToken)
    {
        var countryExists = await _application.Countries
            .AnyAsync(x => x.CountryId == request.CountryId, cancellationToken);

        if (!countryExists)
            return Result<GovernmentDto>
                .Failure(_localizer["CountryNotFound"]);

        var (nameAr, nameEn) =
            NormalizeHelper.Normalize(request.NameAr, request.NameEn);

        var exists = await _application.Governments
            .AnyAsync(x =>
                    (nameAr != null && x.GovernmentNameAr == nameAr) ||
                    (nameEn != null && x.GovernmentNameEn == nameEn),
                cancellationToken);

        if (exists)
            return Result<GovernmentDto>
                .Failure(_localizer["GovernmentAlreadyExists"]);

        var government =
            _mapper.Map<Domain.Entities.Location.Government>(request);

        await _application.Governments
            .AddAsync(government, cancellationToken);

        await _application.SaveChangesAsync(cancellationToken);
        var response = await _application.Governments
            .AsNoTracking()
            .Include(x => x.Country)
            .Where(x => x.GovernmentId == government.GovernmentId)
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
                    : "",

                CreatedAt = x.CreatedAt,

                UpdatedBy = x.UpdatedBy != null
                    ? x.UpdatedBy.FullName
                    : x.UpdatedById,

                UpdatedAt = x.UpdatedAt,

                IsActive = x.IsActive
            })
            .FirstAsync(cancellationToken);

        return Result<GovernmentDto>.Success(
            response,
            _localizer["GovernmentCreatedSuccessfully"]);
    }
}
namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.UpdateCountry;

public sealed record UpdateCountryCommand(
    Guid Id,
    string? NameAr,
    string? NameEn,
    string? PhoneCode
) : IRequest<Result<bool>>;
using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Country.ImportCountries;

public sealed record ImportCountriesCommand(IFormFile File)
    : IRequest<Result<ExcelUploadResult<Domain.Entities.Location.Country>>>;
using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.ImportCities;

public sealed record ImportCitiesCommand(IFormFile File)
    : IRequest<Result<ExcelUploadResult<Domain.Entities.Location.City>>>;
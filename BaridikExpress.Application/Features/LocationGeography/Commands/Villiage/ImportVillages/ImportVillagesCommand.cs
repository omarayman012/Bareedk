using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.ImportVillages;

public sealed record ImportVillagesCommand(IFormFile File)
    : IRequest<Result<ExcelUploadResult<Village>>>;
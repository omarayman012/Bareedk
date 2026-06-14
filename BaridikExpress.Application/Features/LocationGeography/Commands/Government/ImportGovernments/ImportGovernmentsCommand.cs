using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Location;
using Microsoft.AspNetCore.Http;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Government.ImportGovernments;

public sealed record ImportGovernmentsCommand(IFormFile File)
    : IRequest<Result<ExcelUploadResult<Domain.Entities.Location.Government>>>;
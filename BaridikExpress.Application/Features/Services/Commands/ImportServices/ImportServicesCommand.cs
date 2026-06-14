using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Services;

namespace BaridikExpress.Application.Features.Services.Commands.ImportServices
{
    public sealed record ImportServicesCommand(
      IFormFile File)
      : IRequest<Result<ExcelUploadResult<Service>>>;
}

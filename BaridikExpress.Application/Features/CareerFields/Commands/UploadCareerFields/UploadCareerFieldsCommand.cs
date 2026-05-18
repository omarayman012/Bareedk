using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.CareerFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.CareerFields.Commands.UploadCareerFields
{
    public record UploadCareerFieldsCommand(IFormFile File) 
        : IRequest<Result<ExcelUploadResult<CareerField>>>;
}

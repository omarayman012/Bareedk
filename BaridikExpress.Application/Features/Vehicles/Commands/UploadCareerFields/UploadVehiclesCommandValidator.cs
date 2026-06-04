using BaridikExpress.Application.Features.Vehicles.Commands.UploadVehicles;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Vehicles.Commands.UploadCareerFields
{
    public class UploadVehiclesCommandValidator:AbstractValidator<UploadVehiclesCommand>
    {

        public UploadVehiclesCommandValidator(IStringLocalizer localizer)
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage(localizer["ExcelFileRequired"]);

            RuleFor(x => x.File)
                .Must(file =>
                {
                    var extension = Path.GetExtension(file!.FileName)
                        .ToLowerInvariant();

                    return extension is ".xlsx" or ".xls";
                })
                .WithMessage(localizer["ExcelInvalidType"])
                .When(x => x.File != null);
        }

    }
}

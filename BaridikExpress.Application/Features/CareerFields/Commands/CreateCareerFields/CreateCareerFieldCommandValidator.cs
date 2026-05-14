using BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields
{
    public class CreateCareerFieldValidator
        : AbstractValidator<CreateCareerFieldCommand>
    {
        public CreateCareerFieldValidator(
            IStringLocalizer localizer)
        {
            RuleFor(x => x)
               .Must(x =>
                   !string.IsNullOrWhiteSpace(x.NameAr) ||
                   !string.IsNullOrWhiteSpace(x.NameEn)
               )
               .WithMessage(localizer["CareerFieldNameRequired"]);
        }
    }
}
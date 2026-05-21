using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.CareerFields.Commands.UpdateCareerFields
{

    public record UpdateCareerFieldCommand(
        Guid Id,
        string ?NameAr,
        string ?NameEn
    ) : IRequest<Result<bool>>;
}

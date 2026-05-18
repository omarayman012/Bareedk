using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.CareerFields.Commands.DeleteCareerFields
{
    public record DeleteCareerFieldsCommand(
        List<Guid> Ids
    ) : IRequest<Result<List<Guid>>>;
}

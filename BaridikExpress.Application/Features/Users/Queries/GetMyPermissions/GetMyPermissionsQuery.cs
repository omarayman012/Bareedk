using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Users.Queries.GetMyPermissions
{
    public record GetMyPermissionsQuery():IRequest<Result<List<string>>>;
}

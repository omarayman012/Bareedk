using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.Queries.GetPermissions
{
    public record GetPermissionsQuery() : IRequest<Result<List<string>>>;
}

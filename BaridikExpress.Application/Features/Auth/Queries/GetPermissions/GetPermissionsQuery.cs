using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Queries.GetPermissions
{
    public record GetPermissionsQuery() : IRequest<Result<List<PermissionDto>>>;
}

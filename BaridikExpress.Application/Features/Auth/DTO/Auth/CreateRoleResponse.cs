using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.DTO.Auth
{
    public record CreateRoleResponse(
     string Id,
     string Name,
     List<PermissionDto> Permissions
 );
    
}

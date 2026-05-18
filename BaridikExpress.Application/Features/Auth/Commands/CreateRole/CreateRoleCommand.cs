using BaridikExpress.Application.Features.Auth.DTO.Auth;

namespace BaridikExpress.Application.Features.Auth.Commands.CreateRole;

public record CreateRoleCommand(
   string Name,
  List<Guid> PermissionIds 
) : IRequest<Result<CreateRoleResponse>>;

using BaridikExpress.Application.Features.Auth.Commands.CreateRole;
using BaridikExpress.Application.Features.Auth.Commands.DeleteRole;
using BaridikExpress.Application.Features.Auth.Commands.UpdateRole;
using BaridikExpress.Application.Features.Auth.Commands.UpdateRolePermissions;
using BaridikExpress.Application.Features.Auth.DTO.Auth;
using BaridikExpress.Application.Features.Auth.Queries.GetPermissions;
using BaridikExpress.Application.Features.Auth.Queries.GetPermissionsByRole;
using BaridikExpress.Application.Features.Auth.Queries.GetRoles;

namespace BaridikExpress.API.Controllers.RoleModules;

[ApiController]
[Route("api/[controller]")]
public class RolesController(IMediator mediator) : ControllerBase
{
    // GET api/roles
    [HttpGet]
    [HasPermission(Permissions.RolesRead)]
    public async Task<IActionResult> GetRoles()
    {
        var result = await mediator.Send(new GetRolesQuery());
        return StatusCode(result.StatusCode, result);
    }

    // GET api/roles/permissions
    [HttpGet("permissions")]
    [HasPermission(Permissions.RolesRead)]
    public async Task<IActionResult> GetPermissions()
    {
        var result = await mediator.Send(new GetPermissionsQuery());
        return StatusCode(result.StatusCode, result);
    }

    // GET api/roles/{roleId}/permissions
    [HttpGet("{roleId}/permissions")]
    [HasPermission(Permissions.RolesRead)]
    public async Task<IActionResult> GetPermissionsByRole(string roleId)
    {
        var result = await mediator.Send(new GetPermissionsByRoleQuery(roleId));
        return StatusCode(result.StatusCode, result);
    }

    // POST api/roles
    [HttpPost]
    [HasPermission(Permissions.RolesCreate)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }

    // PUT api/roles/{id}
    [HttpPut("{id}")]
    [HasPermission(Permissions.RolesUpdate)]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] CreateRoleRequestDto dto)
    {
        var result = await mediator.Send(new UpdateRoleCommand(id, dto.Name));
        return StatusCode(result.StatusCode, result);
    }

    // PUT api/roles/{roleId}/permissions
    [HttpPut("{roleId}/permissions")]
    [HasPermission(Permissions.RolesUpdate)]
    public async Task<IActionResult> UpdateRolePermissions(string roleId, [FromBody] List<Guid> permissionIds)
    {
        var result = await mediator.Send(new UpdateRolePermissionsCommand(roleId, permissionIds));
        return StatusCode(result.StatusCode, result);
    }

    // DELETE api/roles/{id}
    [HttpDelete("{id}")]
    [HasPermission(Permissions.RolesDelete)]
    public async Task<IActionResult> DeleteRole(string id)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id));
        return StatusCode(result.StatusCode, result);
    }
}
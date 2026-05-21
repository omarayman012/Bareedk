using BaridikExpress.Application.Features.Auth.Queries.GetPermissions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.PermissionModules;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "role-management-v1")]
[Tags("Permissions")]
public class PermissionsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/permissions
    [HttpGet]
    [HasPermission(Permissions.PermissionsRead)]
    public async Task<IActionResult> GetPermissions()
    {
        var result = await _mediator.Send(
            new GetPermissionsQuery());

        return StatusCode(result.StatusCode, result);
    }
}
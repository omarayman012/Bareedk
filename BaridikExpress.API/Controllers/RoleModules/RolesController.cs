using BaridikExpress.Application.Features.Auth.Commands.CreateRole;
using BaridikExpress.Application.Features.Auth.Commands.DeleteRole;
using BaridikExpress.Application.Features.Auth.Commands.UpdateRole;
using BaridikExpress.Application.Features.Auth.DTO.Auth;
using BaridikExpress.Application.Features.Auth.Queries.GetRoles;

namespace BaridikExpress.API.Controllers.RoleModules
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        [HttpGet]
        [HasPermission(Permissions.RolesRead)]
        public async Task<IActionResult> GetRoles()
        {
            var result = await _mediator.Send(new GetRolesQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        [HasPermission(Permissions.RolesCreate)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        [HasPermission(Permissions.RolesUpdate)]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] CreateRoleRequestDto dto)
        {
            var command = new UpdateRoleCommand(id,dto.Name);
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        [HasPermission(Permissions.RolesDelete)]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await _mediator.Send(new DeleteRoleCommand(id));
            return StatusCode(result.StatusCode, result);
        }
    }

}


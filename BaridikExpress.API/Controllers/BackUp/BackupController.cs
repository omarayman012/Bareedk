using BaridikExpress.Application.Features.Backup.Commands.RunBackupNow;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.BackUp
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    [Authorize]
    public class BackupController(IMediator mediator) : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateBackupCommand command)
        {
            var result = await mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
    }
}

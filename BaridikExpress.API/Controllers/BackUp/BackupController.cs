using BaridikExpress.Application.Features.Backup.Commands.RunBackupNow;
using BaridikExpress.Application.Features.Backup.Commands.SaveBackupSettings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.BackUp;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public sealed class BackupController(IMediator mediator) : ControllerBase
{
    #region Run Backup Now

    [HttpPost("run-now")]
    public async Task<IActionResult> RunNow(
        [FromForm] RunBackupNowCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    #endregion

    #region Save Backup Settings

    [HttpPost("settings")]
    public async Task<IActionResult> SaveSettings(
        [FromBody] SaveBackupSettingsCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    #endregion
}
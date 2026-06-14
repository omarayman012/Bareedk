using BaridikExpress.Application.Features.SystemManagement.Commands.UpdateGeneralCompanySettings;
using BaridikExpress.Application.Features.SystemManagement.Queries.GetGeneralCompanySettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.SystemManagement;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class GeneralCompanySettingsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await mediator.Send(new GetGeneralCompanySettingsQuery());
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateGeneralCompanySettingsCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
}
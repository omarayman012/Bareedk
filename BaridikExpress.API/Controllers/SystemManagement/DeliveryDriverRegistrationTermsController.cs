using BaridikExpress.Application.Features.SystemManagement.Commands.UpdateSystemManagement;
using BaridikExpress.Application.Features.SystemManagement.Queries.GetSystemManagement;
using BaridikExpress.Domain.Entities.SystemManagment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.SystemManagement;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]

public class DeliveryDriverRegistrationTermsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetSystemManagementQuery<DeliveryDriverRegistrationTerms>(), cancellationToken);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        [FromBody] UpdateSystemManagementCommand<DeliveryDriverRegistrationTerms> command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return StatusCode(result.StatusCode, result);
    }
}
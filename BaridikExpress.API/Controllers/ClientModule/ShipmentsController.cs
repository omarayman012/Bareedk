using BaridikExpress.Application.Features.Shipments.Commands.CreateShipment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.AuthClientModule;

[ApiController]
[Route("api/client/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "client-v1")]
[Tags("Shipments")]
public class ShipmentsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create(
        [FromForm] CreateShipmentCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(result.StatusCode, result);
    }
}
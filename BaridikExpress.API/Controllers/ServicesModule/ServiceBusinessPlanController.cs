using BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Create;
using BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Delete;
using BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.ToggleStatus;
using BaridikExpress.Application.Features.ServiceBusinessPlans.Commands.Update;
using BaridikExpress.Application.Features.ServiceBusinessPlans.Queries.GetAll;
using BaridikExpress.Application.Features.ServiceBusinessPlans.Queries.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.ServicesModule;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]
public class ServiceBusinessPlanController(IMediator mediator) : ControllerBase
{
    [HttpGet("GetAll")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllServiceBusinessPlansQuery query)
    {
        var result = await mediator.Send(query);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(
        [FromForm] CreateServiceBusinessPlanCommand command)
    {
        var result = await mediator.Send(command);
        return StatusCode(result.StatusCode, result);
    }
    [HttpGet("GetById/{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(
        new GetServiceBusinessPlanByIdQuery(id));

       return StatusCode(result.StatusCode, result);
    }

    [HttpPut("Update/{id:guid}")]
    public async Task<IActionResult> Update(
             Guid id,
          [FromForm] UpdateServiceBusinessPlanCommand command)
    {
        command.Id = id;

        var result = await mediator.Send(command);

        return StatusCode(result.StatusCode, result);

    }
    [HttpDelete("DeleteList")]
    public async Task<IActionResult> DeleteList(
[FromBody] DeleteServiceBusinessPlansCommand command)
    {
        var result = await mediator.Send(command);

         return StatusCode(result.StatusCode, result);

    }
    [HttpPatch("ToggleStatus/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await mediator.Send(
        new ToggleServiceBusinessPlanStatusCommand(id));

        return StatusCode(result.StatusCode, result);

    }


}
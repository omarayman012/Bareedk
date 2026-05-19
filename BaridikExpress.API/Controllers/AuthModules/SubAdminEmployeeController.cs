using BaridikExpress.Application.Features.Auth.Commands.CreateSubAdminEmployee;
using BaridikExpress.Application.Features.Auth.Commands.DeleteSubAdminEmployee;
using BaridikExpress.Application.Features.Auth.Commands.ToggleSubAdminEmployeeStatus;
using BaridikExpress.Application.Features.Auth.Commands.UpdateSubAdminEmployee;
using BaridikExpress.Application.Features.Auth.Queries.GetAllSubAdminEmployees;
using BaridikExpress.Application.Features.Auth.Queries.GetSubAdminEmployeeById;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubAdminEmployeeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllSubAdminEmployeesQuery query)
    {
        var result = await _mediator.Send(query);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetSubAdminEmployeeByIdQuery { Id = id });
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] CreateSubAdminEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? StatusCode(201, result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateSubAdminEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteSubAdminEmployeeCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _mediator.Send(new ToggleSubAdminEmployeeStatusCommand { Id = id });
        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }
}
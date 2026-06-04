using BaridikExpress.Application.Features.Auth.Commands.CreateSubAdminEmployee;
using BaridikExpress.Application.Features.Auth.Commands.DeleteSubAdminEmployee;
using BaridikExpress.Application.Features.Auth.Commands.ToggleSubAdminEmployeeStatus;
using BaridikExpress.Application.Features.Auth.Commands.UpdateSubAdminEmployee;
using BaridikExpress.Application.Features.Auth.Queries.GetAllSubAdminEmployees;
using BaridikExpress.Application.Features.Auth.Queries.GetSubAdminEmployeeById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.Auth;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[ApiExplorerSettings(GroupName = "auth-v1")]
[Tags("Sub Admin Employees")]
public class SubAdminEmployeeController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    // GET: api/subadminemployee
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllSubAdminEmployeesQuery query)
    {
        var result = await _mediator.Send(query);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // GET: api/subadminemployee/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(
            new GetSubAdminEmployeeByIdQuery
            {
                Id = id
            });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // POST: api/subadminemployee
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromForm] CreateSubAdminEmployeeCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, result)
            : StatusCode(result.StatusCode, result);
    }

    // PUT: api/subadminemployee
    [HttpPut]
    public async Task<IActionResult> Update(
        [FromForm] UpdateSubAdminEmployeeCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // DELETE: api/subadminemployee
    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromBody] DeleteSubAdminEmployeeCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }

    // PATCH: api/subadminemployee/toggle-status/{id}
    [HttpPatch("toggle-status/{id:guid}")]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var result = await _mediator.Send(
            new ToggleSubAdminEmployeeStatusCommand
            {
                Id = id
            });

        return result.IsSuccess
            ? Ok(result)
            : StatusCode(result.StatusCode, result);
    }
}
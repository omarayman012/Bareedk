using BaridikExpress.Application.Features.Statistics.Queries.GetStatistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "admin-v1")]
[Authorize]


public class StatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetStatistics(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetStatisticsQuery(), cancellationToken);
        return Ok(result);
    }
}
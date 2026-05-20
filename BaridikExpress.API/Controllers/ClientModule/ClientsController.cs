using BaridikExpress.Application.Features.ClientModule.Command;
using BaridikExpress.Application.Features.ClientModule.Queries;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.ClientModule
{
    [ApiController]
    [Route("api/client/[controller]")]  
    [ApiExplorerSettings(GroupName = "client-v1")]   
    public class ClientsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterClientCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] Guid? careerFieldId = null,
            [FromQuery] DateTime? createdFrom = null,
            [FromQuery] DateTime? createdTo = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllClientsQuery
            {
                Search = search,
                CareerFieldId = careerFieldId,
                CreatedFrom = createdFrom,
                CreatedTo = createdTo,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result);

            return Ok(result);
        }
    }
} 

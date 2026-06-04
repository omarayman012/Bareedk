using BaridikExpress.Application.Features.Auth.Commands.ResetPassword;
using BaridikExpress.Application.Features.AuthClientModule.Queries;
using BaridikExpress.Application.Features.ClientModule.Commond;
using BaridikExpress.Application.Features.ClientModule.DTOs;
using BaridikExpress.Application.Features.ClientModule.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.AuthClientModule
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

        [Authorize(Roles = "SuperAdmin")]
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
            return StatusCode(result.StatusCode, result);
        }

      
        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(
            [FromBody] DeleteClientCommand command)
        {
            var result = await _mediator.Send(command);

            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetClientByIdQuery { Id = id });
            return StatusCode(result.StatusCode, result);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateClientDto dto)
        {
            var result = await _mediator.Send(new UpdateClientCommand
            {
                Dto = dto
            });

            return StatusCode(result.StatusCode, result);
        }
    }
}
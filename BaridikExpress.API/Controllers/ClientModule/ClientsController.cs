using BaridikExpress.Application.Features.Auth.Commands.ResetPassword;
using BaridikExpress.Application.Features.AuthClientModule.Command;
using BaridikExpress.Application.Features.AuthClientModule.Queries;
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

        [Authorize("SuperAdmin")]
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


     
    }
}
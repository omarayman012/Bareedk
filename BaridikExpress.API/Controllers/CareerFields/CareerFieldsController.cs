using BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.CareerFields
{
        [ApiController]
        [Route("api/[controller]")]
        public class CareerFieldsController(IMediator mediator) : ControllerBase
        {
            private readonly IMediator _mediator = mediator;
            [HttpPost]
          //  [HasPermission(Permissions.CareerFieldsCreate)]
            public async Task<IActionResult> Create(
                [FromBody] CreateCareerFieldCommand command)
            {
                var result = await _mediator.Send(command);
                return StatusCode(result.StatusCode, result);
            }
        }
}


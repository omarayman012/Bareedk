using BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields;
using BaridikExpress.Application.Features.CareerFields.Commands.ToggleCareerFieldStatus;
using BaridikExpress.Application.Features.CareerFields.Commands.UpdateCareerFields;
using BaridikExpress.Application.Features.CareerFields.Queries.GetAllCareerFields;
using BaridikExpress.Application.Features.CareerFields.Queries.GetCareerFieldById;

namespace BaridikExpress.API.Controllers.CareerFields
{
        [ApiController]
        [Route("api/[controller]")]
        public class CareerFieldsController(IMediator mediator) : ControllerBase
        {
            private readonly IMediator _mediator = mediator;



        [HttpGet]
        // [HasPermission(Permissions.CareerFieldsRead)]
        public async Task<IActionResult> GetAll(
         [FromQuery] GetAllCareerFieldsQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        // [HasPermission(Permissions.CareerFieldsRead)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(
                new GetCareerFieldByIdQuery(id)
            );
            return StatusCode(result.StatusCode, result);
        }


        [HttpPost]
          //[HasPermission(Permissions.CareerFieldsCreate)]
            public async Task<IActionResult> Create(
                [FromBody] CreateCareerFieldCommand command)
            {
                var result = await _mediator.Send(command);
                return StatusCode(result.StatusCode, result);
            }
        [HttpPut]
       // [HasPermission(Permissions.CareerFieldsUpdate)]
        public async Task<IActionResult> Update(
            [FromBody] UpdateCareerFieldCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        //[HttpDelete("{id:guid}")]
        ////[HasPermission(Permissions.CareerFieldsDelete)]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var result = await _mediator.Send(
        //        new DeleteCareerFieldCommand(id)
        //    );

        //    return StatusCode(result.StatusCode, result);
        //}

        [HttpPatch("{id}/toggle-status")]
        //[HasPermission(Permissions.CareerFieldsUpdate)]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await _mediator.Send(
                new ToggleCareerFieldStatusCommand(id)
            );

            return StatusCode(result.StatusCode, result);
        }

    }
}


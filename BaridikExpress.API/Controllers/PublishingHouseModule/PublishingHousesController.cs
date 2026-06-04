using BaridikExpress.Application.Features.PublishingHouseModule.Command;
using BaridikExpress.Application.Features.PublishingHouseModule.Dto;
using BaridikExpress.Application.Features.PublishingHouseModule.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.PublishingHouseModule
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    public sealed class PublishingHousesController(IMediator mediator) : ControllerBase
    {
        
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Result<PublishingHouseDto>>> Create(
            [FromForm] CreatePublishingHouseCommand command, 
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<ActionResult<Result<PaginatedList<PublishingHouseGetAllDto>>>> GetAll(
          [FromQuery] GetAllPublishingHouseQuery query,
          CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Result<PublishingHouseDetailsDto>>> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetByIdPublishingHouseQuery { Id = id }, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<ActionResult<Result<UpdatePublishingHouseResponseDto>>> Update(
             [FromForm] UpdatePublishingHouseCommand command,
             CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<ActionResult<Result<bool>>> Delete(
            [FromBody] DeletePublishingHouseCommand command,
            CancellationToken cancellationToken) 
        {
            var result = await _mediator.Send(command, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPatch("change-status/{id:guid}")]
        public async Task<ActionResult<Result<string>>> ChangeStatus(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(
                new ChangePublishingHouseStatusCommand
                {
                    Id = id
                },
                cancellationToken);

            return StatusCode(result.StatusCode, result);
        }
    }

}

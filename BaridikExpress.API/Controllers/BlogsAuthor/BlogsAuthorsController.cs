using BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Commands;
using BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Domain.Enum;
using Microsoft.AspNetCore.Authorization;

namespace BaridikExpress.API.Controllers.BlogsAuthor
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    [Authorize]
    public class BlogsAuthorsController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<Result<object>>> GetAll(
            [FromQuery] string? Name,
            [FromQuery] string? Email,
            [FromQuery] string? PhoneNumber,
            [FromQuery] Guid? CategoryId,
            [FromQuery] string? CreatedById,
            [FromQuery] UserGender? Gender,
            [FromQuery] bool? IsActive,
            [FromQuery] DateTime? FromDate,
            [FromQuery] DateTime? ToDate,
            [FromQuery] int? pageSize = 10,
            [FromQuery] int pageNumber = 1)
        {
            var query = new GetAllBlogsAuthorsQuery
            {
                Name = Name,
                Email = Email,
                PhoneNumber = PhoneNumber,
                CategoryId = CategoryId,
                CreatedById = CreatedById,
                Gender = Gender,
                IsActive = IsActive,
                FromDate = FromDate,
                ToDate = ToDate,
                pageSize = pageSize,
                PageNumber = pageNumber
            };

            var result = await mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<GetBlogsAuthorByIdDto>>> GetById(Guid id)
        {
            var result = await mediator.Send(new GetBlogsAuthorByIdQuery { Id = id });
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<Result<ResponseBlogsAuthorDto>>> Create(
            [FromForm] CreateBlogsAuthorCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.Message);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] UpdateBlogsAuthorCommand command)
        {
            command.Id = id;
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.Message);

            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteBlogsAuthorCommand command)
        {
            var result = await mediator.Send(command);
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.Message);

            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("toggle-status/{id}")]
        public async Task<IActionResult> ToggleStatus(Guid id)
        {
            var result = await mediator.Send(new ToggleBlogsAuthorStatusCommand { Id = id });
            if (!result.IsSuccess)
                return StatusCode(result.StatusCode, result.Message);

            return StatusCode(result.StatusCode, result);
        }
    }
}

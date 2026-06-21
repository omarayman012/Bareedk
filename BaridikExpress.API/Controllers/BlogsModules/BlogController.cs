using BaridikExpress.Application.Features.BlogsModules.Blogs.Commands;
using BaridikExpress.Application.Features.BlogsModules.Blogs.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaridikExpress.API.Controllers.BlogsModules
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = "admin-v1")]
    //[Authorize]
    public class BlogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateBlogCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateBlogCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetBlogsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetBlogByIdQuery
            {
                Id = id,
                CommentsPageNumber = pageNumber,
                CommentsPageSize = pageSize
            };

            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("ToggleStatus/{id:guid}")]
        public async Task<IActionResult> ToggleStatus(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new ToggleBlogStatusCommand { BlogId = id }, cancellationToken);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("reaction")]
        public async Task<IActionResult> React([FromBody] ToggleBlogReactionCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteBlogsCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("reactions/{blogId}")]
        public async Task<IActionResult> GetReactions(Guid blogId)
        {
            var result = await _mediator.Send(new GetBlogReactionsQuery { BlogId = blogId });
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("favorites")]
        public async Task<IActionResult> GetFavorites(
              [FromQuery] GetFavoriteBlogsQuery query)
        {
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("details/{id:guid}")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var result = await _mediator.Send(new GetBlogDetailsQuery
            {
                BlogId = id
            });

            return StatusCode(result.StatusCode, result);
        }
    }
}

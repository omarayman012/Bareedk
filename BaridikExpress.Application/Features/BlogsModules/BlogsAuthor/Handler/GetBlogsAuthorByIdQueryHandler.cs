
using BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using MediatR;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Handler;

public sealed class GetBlogsAuthorByIdQueryHandler
    : IRequestHandler<GetBlogsAuthorByIdQuery, Result<GetBlogsAuthorByIdDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IStringLocalizer _localizer;
    private readonly ILogger<GetBlogsAuthorByIdQueryHandler> _logger;

    public GetBlogsAuthorByIdQueryHandler(
        IApplicationDbContext context,
        IStringLocalizer localizer, 
        ILogger<GetBlogsAuthorByIdQueryHandler> logger)
    {
        _context = context;
        _localizer = localizer;
        _logger = logger;
    }

    public async Task<Result<GetBlogsAuthorByIdDto>> Handle(
        GetBlogsAuthorByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var dto = await _context.BlogsAuthors
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new GetBlogsAuthorByIdDto
                {
                    Id = x.Id,
                    Name = new LocalizedDto
                    {
                        AR = x.NameAr ?? string.Empty,
                        EN = x.NameEn ?? string.Empty
                    },
                    Gender = x.Gender.ToString(),
                    Email = x.Email ?? string.Empty,        
                    PhoneNumber = x.PhoneNumber ?? string.Empty,
                    Image = x.Image ?? string.Empty,           
                    Category = x.BlogsCategory == null         
                        ? new LocalizedNameDto()
                        : new LocalizedNameDto
                        {
                            Id = x.BlogsCategory.Id,
                            AR = x.BlogsCategory.NameAr ?? string.Empty,
                            EN = x.BlogsCategory.NameEn ?? string.Empty
                        },
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : null,
                    UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FullName : null,
                    BlogsCount = x.Blogs.Count,
                    Blogs = x.Blogs
                        .OrderByDescending(b => b.CreatedAt)
                        .Select(b => new AuthorBlogDto
                        {
                            Id = b.Id,
                            Title = new LocalizedDto
                            {
                                AR = b.TitleAr ?? string.Empty,
                                EN = b.TitleEn ?? string.Empty
                            },
                            ImageUrl = b.Image ?? string.Empty,
                            IsActive = b.IsActive,
                            CreatedAt = b.CreatedAt
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (dto is null)
                return Result<GetBlogsAuthorByIdDto>.Failure(
                    _localizer["BlogsAuthorNotFound"], 404);

            return Result<GetBlogsAuthorByIdDto>.Success(
                dto,
                _localizer["BlogsAuthorsRetrievedSuccessfully"],
                200);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error retrieving blog author with Id {AuthorId}",
                request.Id);

            return Result<GetBlogsAuthorByIdDto>.Error(
                _localizer["FailedToRetrieveBlogsAuthors"],
                500);
        }
    }
}

using BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Queries;
using BaridikExpress.Application.Features.BlogsModules.DTOs;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsAuthor.Handler
{
    public class GetAllBlogsAuthorsQueryHandler
        : IRequestHandler<GetAllBlogsAuthorsQuery, Result<PaginatedList<GetAllBlogsAuthorDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly ILogger<GetAllBlogsAuthorsQueryHandler> _logger;

        public GetAllBlogsAuthorsQueryHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            ILogger<GetAllBlogsAuthorsQueryHandler> logger)
        {
            _context = context;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<Result<PaginatedList<GetAllBlogsAuthorDto>>> Handle(
            GetAllBlogsAuthorsQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                var pageSize = request.pageSize.HasValue && request.pageSize.Value > 0
                    ? request.pageSize.Value
                    : 10;

                var query = _context.BlogsAuthors
                    .AsNoTracking()
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    var name = request.Name.Trim().ToLower();

                    query = query.Where(x =>
                        (x.NameAr != null && x.NameAr.ToLower().Contains(name)) ||
                        (x.NameEn != null && x.NameEn.ToLower().Contains(name)));
                }

                if (!string.IsNullOrWhiteSpace(request.Email))
                {
                    var email = request.Email.Trim().ToLower();

                    query = query.Where(x =>
                        x.Email != null && x.Email.ToLower().Contains(email));
                }

                if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
                {
                    var phoneNumber = request.PhoneNumber.Trim();
                    query = query.Where(x => x.PhoneNumber.Contains(phoneNumber));
                }

                if (request.CategoryId.HasValue)
                {
                    query = query.Where(x => x.BlogsCategoryId == request.CategoryId.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.CreatedById))
                {
                    var userId = await _context.SubAdminEmployees
                        .AsNoTracking()
                        .Where(e => e.Id.ToString() == request.CreatedById)
                        .Select(e => e.UserId)
                        .FirstOrDefaultAsync(cancellationToken);

                    if (userId != null)
                        query = query.Where(x => x.CreatedById == userId);
                }

                if (request.Gender.HasValue)
                {
                    query = query.Where(x => x.Gender == request.Gender.Value);
                }

                if (request.IsActive.HasValue)
                {
                    query = query.Where(x => x.IsActive == request.IsActive.Value);
                }

                if (request.FromDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    var toDate = request.ToDate.Value.Date.AddDays(1);
                    query = query.Where(x => x.CreatedAt < toDate);
                }

                var selectQuery = query
                    .OrderByDescending(x => x.CreatedAt)
                    .Select(x => new GetAllBlogsAuthorDto
                    {
                        Id = x.Id,
                        Name = new LocalizedDto
                        {
                            AR = x.NameAr,
                            EN = x.NameEn
                        },
                        Gender = x.Gender.ToString(),
                        Email = x.Email,
                        PhoneNumber = x.PhoneNumber,
                        Image = x.Image,
                        CategoryName = new LocalizedNameDto
                        {
                            Id = x.BlogsCategory.Id,
                            AR = x.BlogsCategory.NameAr,
                            EN = x.BlogsCategory.NameEn
                        },
                        BlogsCount = x.Blogs.Count,
                        IsActive = x.IsActive,
                        CreatedAt = x.CreatedAt,
                        UpdatedAt = x.UpdatedAt,
                        CreatedBy = x.CreatedBy != null ? x.CreatedBy.FullName : null,
                        UpdatedBy = x.UpdatedBy != null ? x.UpdatedBy.FullName : null
                    });

                var paginatedList = await PaginatedList<GetAllBlogsAuthorDto>.CreateAsync(
                    selectQuery,
                    request.PageNumber,
                    pageSize);

                return Result<PaginatedList<GetAllBlogsAuthorDto>>.Success(
                    paginatedList,
                    _localizer["BlogsAuthorsRetrievedSuccessfully"],
                    200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving blog authors");

                return Result<PaginatedList<GetAllBlogsAuthorDto>>.Error(
                    _localizer["FailedToRetrieveBlogsAuthors"],
                    500);
            }
        }
    }
}
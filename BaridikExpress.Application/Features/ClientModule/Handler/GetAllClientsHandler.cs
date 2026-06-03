using BaridikExpress.Application.Features.ClientModule.DTOs;
using BaridikExpress.Application.Features.AuthClientModule.Queries;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ClientModule.Handler
{
    public class GetAllClientsHandler
      : IRequestHandler<GetAllClientsQuery, Result<PagedResult<GetAllClientsDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllClientsHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<PagedResult<GetAllClientsDto>>> Handle(
            GetAllClientsQuery request,
            CancellationToken cancellationToken)
        {
            // ================= AUTH CHECK =================
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<PagedResult<GetAllClientsDto>>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            // ================= QUERY =================
            var query = _context.Clients
                .Include(c => c.User)
                .Include(c => c.CareerField)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();
                query = query.Where(c =>
                    c.User.FullName.Contains(search) ||
                    c.User.Email.Contains(search) ||
                    c.User.PhoneNumber.Contains(search));
            }

            if (request.CareerFieldId.HasValue && request.CareerFieldId.Value != Guid.Empty)
            {
                query = query.Where(c => c.CareerFieldId == request.CareerFieldId);
            }

            if (request.CreatedFrom.HasValue)
                query = query.Where(c => c.CreatedAt >= request.CreatedFrom);

            if (request.CreatedTo.HasValue)
                query = query.Where(c => c.CreatedAt <= request.CreatedTo);

            // ================= PROJECTION =================
            var projected = query.Select(c => new GetAllClientsDto
            {
                Id = c.Id,
                FullName = c.User.FullName,
                Email = c.User.Email!,
                Phone = c.User.PhoneNumber!,
                CareerFieldName = new LocalizedDto
                {
                    EN = c.CareerField.Name.En,
                    AR = c.CareerField.Name.Ar
                },
                CompanyName = c.CompanyName,
                CompanyLink = c.CompanyLink,
                AcceptTerms = c.AcceptTerms,
                AcceptPrivacy = c.AcceptPrivacy,
                CreatedAt = c.CreatedAt,
                Role = "Client"
            });

            // ================= COUNT =================
            var totalCount = await projected.CountAsync(cancellationToken);

            // ================= PAGINATION =================
            var items = await projected
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // ================= RESULT =================
            var resultData = new PagedResult<GetAllClientsDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.PageNumber,
                PageSize = request.PageSize
            };

            return Result<PagedResult<GetAllClientsDto>>.Success(
                resultData,
                _localizer["Success"],
                200);
        }
    }
}
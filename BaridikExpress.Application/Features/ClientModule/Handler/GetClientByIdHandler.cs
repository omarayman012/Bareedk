using BaridikExpress.Application.Features.ClientModule.DTOs;
using BaridikExpress.Application.Features.ClientModule.Queries;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.ClientModule.Handler
{
    public class GetClientByIdHandler
       : IRequestHandler<GetClientByIdQuery, Result<GetClientByIdDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetClientByIdHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<GetClientByIdDto>> Handle(
            GetClientByIdQuery request,
            CancellationToken cancellationToken)
        {
            // ================= AUTH CHECK =================
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<GetClientByIdDto>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            // ================= GET CLIENT =================
            var client = await _context.Clients
                .Include(c => c.User)
                .Include(c => c.CareerField)
                .FirstOrDefaultAsync(
                    c => c.UserId == request.Id,
                    cancellationToken);

            if (client == null)
            {
                return Result<GetClientByIdDto>.Failure(
                    _localizer["NotFound"],
                    404);
            }

            // ================= DTO =================
            var dto = new GetClientByIdDto
            {
                Id = client.UserId,
                FullName = client.User.FullName,
                Email = client.User.Email!,
                Phone = client.User.PhoneNumber!,

                CareerFieldName = new LocalizedDto
                {
                    EN = client.CareerField.Name.En,
                    AR = client.CareerField.Name.Ar
                },

                CompanyName = client.CompanyName,
                CompanyLink = client.CompanyLink,

                AcceptTerms = client.AcceptTerms,
                AcceptPrivacy = client.AcceptPrivacy,
                CreatedAt = client.CreatedAt
            };

            return Result<GetClientByIdDto>.Success(
                dto,
                _localizer["Success"],
                200);
        }
    }
}
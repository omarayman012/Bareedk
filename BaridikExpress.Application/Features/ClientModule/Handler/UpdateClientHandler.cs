using BaridikExpress.Application.Features.ClientModule.Commond;
using BaridikExpress.Application.Features.ClientModule.DTOs;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ClientModule.Handler
{
    public class UpdateClientHandler
     : IRequestHandler<UpdateClientCommand, Result<GetClientByIdDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateClientHandler(
            IApplicationDbContext context,
            UserManager<User> userManager,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<GetClientByIdDto>> Handle(
            UpdateClientCommand request,
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

            var dto = request.Dto;

            // ================= GET CLIENT =================
            var client = await _context.Clients
                 .Include(c => c.User)
                 .Include(c => c.CareerField)
                 .FirstOrDefaultAsync(c => c.UserId == dto.UserId, cancellationToken);

            if (client == null)
            {
                return Result<GetClientByIdDto>.Failure(
                    _localizer["NotFound"],
                    404);
            }

            // ================= UPDATE USER =================
            client.User.FullName = dto.FullName;

            // ================= UPDATE CLIENT =================
            client.CareerFieldId = dto.CareerFieldId;
            client.CompanyName = dto.CompanyName;
            client.CompanyLink = dto.CompanyLink;
            client.AcceptTerms = true;
            client.AcceptPrivacy = true;

            await _userManager.UpdateAsync(client.User);
            await _context.SaveChangesAsync(cancellationToken);

            // ================= RESPONSE DTO =================
            var resultDto = new GetClientByIdDto
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
                resultDto,
                _localizer["UpdatedSuccessfully"],
                200);
        }
    }
}
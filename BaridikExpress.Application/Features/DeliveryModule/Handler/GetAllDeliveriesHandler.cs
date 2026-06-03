using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Application.Features.DeliveryModule.Queries;
using BaridikExpress.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class GetAllDeliveriesHandler
        : IRequestHandler<GetAllDeliveriesQuery, Result<PagedResult<GetAllDeliveriesDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllDeliveriesHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<PagedResult<GetAllDeliveriesDto>>> Handle(
            GetAllDeliveriesQuery request,
            CancellationToken cancellationToken)
        {
            // ================= AUTH CHECK =================
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<PagedResult<GetAllDeliveriesDto>>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            // ================= QUERY =================
            var query = _context.Deliveries
                .Include(x => x.User)
                .AsQueryable();

            // ================= SEARCH =================
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.User.FullName.Contains(request.Search) ||
                    x.User.Email.Contains(request.Search) ||
                    x.User.PhoneNumber.Contains(request.Search));
            }

            // ================= APPROVAL FILTER =================
            if (request.IsApproved.HasValue)
                query = query.Where(x => x.IsApproved == request.IsApproved);

            // ================= DATE FILTER =================
            if (request.ApprovedFrom.HasValue)
                query = query.Where(x => x.ApprovedAt >= request.ApprovedFrom);

            if (request.ApprovedTo.HasValue)
                query = query.Where(x => x.ApprovedAt <= request.ApprovedTo);

            // ================= LOCATION FILTERS =================
            if (!string.IsNullOrWhiteSpace(request.Country))
                query = query.Where(x => x.Country == request.Country);

            if (!string.IsNullOrWhiteSpace(request.Gov))
                query = query.Where(x => x.Gov == request.Gov);

            if (!string.IsNullOrWhiteSpace(request.City))
                query = query.Where(x => x.City == request.City);

            if (!string.IsNullOrWhiteSpace(request.Village))
                query = query.Where(x => x.Village == request.Village);

            // ================= PROJECTION =================
            var projected = query.Select(x => new GetAllDeliveriesDto
            {
                Id = x.UserId,
                FullName = x.User.FullName,
                Email = x.User.Email!,
                Phone = x.User.PhoneNumber!,
                DateOfBirth = x.DateOfBirth,

                PlateNo = x.PlateNo,
                VehType = x.VehType.ToString(),

                IsApproved = x.IsApproved,
                ApprovedAt = x.ApprovedAt,
                CreateType = x.CreateType.ToString(),

                Country = x.Country,
                Gov = x.Gov,
                City = x.City,
                Village = x.Village,
                Address = x.Address,
                Floor = x.Floor,
                Apt = x.Apt,

                Edu = x.Edu,

                ProfileImg = x.ProfileImg,
                NidImg = x.NidImg,
                LicImg = x.LicImg,
                VehImg = x.VehImg,
                PoliceCertImg = x.PoliceCertImg,
                PlateImg = x.PlateImg,

                TermsAccepted = x.TermsAccepted,
                PrivacyAccepted = x.PrivacyAccepted
            });

            // ================= PAGINATION =================
            var count = await projected.CountAsync(cancellationToken);

            var items = await projected
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var resultData = new PagedResult<GetAllDeliveriesDto>
            {
                Items = items,
                TotalCount = count,
                Page = request.PageNumber,
                PageSize = request.PageSize
            };

            return Result<PagedResult<GetAllDeliveriesDto>>.Success(
                resultData,
                _localizer["Success"],
                200);
        }
    }
}
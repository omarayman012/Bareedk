using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Application.Features.DeliveryModule.Queries;
using BaridikExpress.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class GetAllDeliveriesHandler
        : IRequestHandler<GetAllDeliveriesQuery, Result<PagedResult<GetAllDeliveriesDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public GetAllDeliveriesHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<PagedResult<GetAllDeliveriesDto>>> Handle(
            GetAllDeliveriesQuery request,
            CancellationToken cancellationToken)
        {
            var query = _context.Deliveries
                .Include(x => x.User)
                .AsQueryable();

            // 🔍 Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.User.FullName.Contains(request.Search) ||
                    x.User.Email.Contains(request.Search) ||
                    x.User.PhoneNumber.Contains(request.Search));
            }

            // ✔️ Approval
            if (request.IsApproved.HasValue)
                query = query.Where(x => x.IsApproved == request.IsApproved);

            // 📅 Date Range
            if (request.ApprovedFrom.HasValue)
                query = query.Where(x => x.ApprovedAt >= request.ApprovedFrom);

            if (request.ApprovedTo.HasValue)
                query = query.Where(x => x.ApprovedAt <= request.ApprovedTo);

            // 📍 Location Filters
            if (!string.IsNullOrWhiteSpace(request.Country))
                query = query.Where(x => x.Country == request.Country);

            if (!string.IsNullOrWhiteSpace(request.Gov))
                query = query.Where(x => x.Gov == request.Gov);

            if (!string.IsNullOrWhiteSpace(request.City))
                query = query.Where(x => x.City == request.City);

            if (!string.IsNullOrWhiteSpace(request.Village))
                query = query.Where(x => x.Village == request.Village);

            // 📌 Projection
            var projected = query.Select(x => new GetAllDeliveriesDto
            {
                Id = x.Id,
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

            // 📄 Pagination
            var count = await projected.CountAsync(cancellationToken);

            var items = await projected
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // ✅ Build PagedResult manually (بدون Create)
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
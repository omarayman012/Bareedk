using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
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
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<PagedResult<GetAllDeliveriesDto>>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            var query = _context.Deliveries
                .Include(x => x.User)
                .Include(x => x.Country)
                .Include(x => x.Government)
                .Include(x => x.City)
                .Include(x => x.Village)
                .AsQueryable();

            // ================= SEARCH =================
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                query = query.Where(x =>
                    x.User.FullName.Contains(request.Search) ||
                    x.User.Email.Contains(request.Search) ||
                    x.User.PhoneNumber.Contains(request.Search));
            }

            // ================= APPROVAL =================
            if (request.IsApproved.HasValue)
                query = query.Where(x => x.IsApproved == request.IsApproved);

            // ================= DATE =================
            if (request.ApprovedFrom.HasValue)
                query = query.Where(x => x.ApprovedAt >= request.ApprovedFrom);

            if (request.ApprovedTo.HasValue)
                query = query.Where(x => x.ApprovedAt <= request.ApprovedTo);

            // ================= LOCATION FILTERS =================
            if (request.Country.HasValue)
                query = query.Where(x => x.CountryId == request.Country);

            if (request.Gov.HasValue)
                query = query.Where(x => x.GovernmentId == request.Gov);

            if (request.City.HasValue)
                query = query.Where(x => x.CityId == request.City);

            if (request.Village.HasValue)
                query = query.Where(x => x.VillageId == request.Village);

            // ================= PROJECTION =================
            var projected = query.Select(x => new GetAllDeliveriesDto
            {
                Id = x.UserId,
                FullName = x.User.FullName,
                Email = x.User.Email!,
                Phone = x.User.PhoneNumber!,
                IsEmailVerified = x.User.EmailConfirmed,
                IsPhoneVerified = x.User.PhoneNumberConfirmed,
                DateOfBirth = x.DateOfBirth,

                PlateNo = x.PlateNo,
                VehType = x.VehType.ToString(),

                IsApproved = x.IsApproved,
                ApprovedAt = x.ApprovedAt,
                CreateType = x.CreateType.ToString(),

                Country = x.Country == null ? null : new LocalizedNameDto
                {
                    Id = x.Country.Id,
                    EN = x.Country.NameEn,
                    AR = x.Country.NameAr
                },

                Gov = x.Government == null ? null : new LocalizedNameDto
                {
                    Id = x.Government.Id,
                    EN = x.Government.NameEn,
                    AR = x.Government.NameAr
                },

                City = x.City == null ? null : new LocalizedNameDto
                {
                    Id = x.City.Id,
                    EN = x.City.NameEn,
                    AR = x.City.NameAr
                },

                Village = x.Village == null ? null : new LocalizedNameDto
                {
                    Id = x.Village.Id,
                    EN = x.Village.NameEn,
                    AR = x.Village.NameAr
                },

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

            var count = await projected.CountAsync(cancellationToken);

            var items = await projected
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var result = new PagedResult<GetAllDeliveriesDto>
            {
                Items = items,
                TotalCount = count,
                Page = request.PageNumber,
                PageSize = request.PageSize
            };

            return Result<PagedResult<GetAllDeliveriesDto>>.Success(
                result,
                _localizer["Success"],
                200);
        }
    }
}
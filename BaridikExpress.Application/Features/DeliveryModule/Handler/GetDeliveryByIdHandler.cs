using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.DeliveryModule.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class GetDeliveryByIdHandler
        : IRequestHandler<GetDeliveryByIdQuery, Result<GetDeliveryByIdDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetDeliveryByIdHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _localizer = localizer;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<GetDeliveryByIdDto>> Handle(
            GetDeliveryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null || !user.Identity?.IsAuthenticated == true)
            {
                return Result<GetDeliveryByIdDto>.Failure(
                    _localizer["Unauthorized"],
                    401);
            }

            var delivery = await _context.Deliveries
                .Include(x => x.User)
                .Include(x => x.Country)
                .Include(x => x.Government)
                .Include(x => x.City)
                .Include(x => x.Village)
                .Where(x => x.UserId == request.Id)
                .Select(x => new GetDeliveryByIdDto
                {
                    Id = x.UserId,
                    FullName = x.User.FullName,
                    Email = x.User.Email!,
                    Phone = x.User.PhoneNumber!,

                    DateOfBirth = x.DateOfBirth,

                    // VEHICLE
                    PlateNo = x.PlateNo,
                    VehType = x.VehType.ToString(),

                    // APPROVAL
                    IsApproved = x.IsApproved,
                    ApprovedAt = x.ApprovedAt,
                    CreateType = x.CreateType.ToString(),

                    // ADDRESS (FIXED)
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

                    // FILES
                    ProfileImg = x.ProfileImg,
                    NidImg = x.NidImg,
                    LicImg = x.LicImg,
                    VehImg = x.VehImg,
                    PoliceCertImg = x.PoliceCertImg,
                    PlateImg = x.PlateImg,

                    // TERMS
                    TermsAccepted = x.TermsAccepted,
                    PrivacyAccepted = x.PrivacyAccepted
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (delivery == null)
            {
                return Result<GetDeliveryByIdDto>.Failure(
                    _localizer["DeliveryNotFound"],
                    404);
            }

            return Result<GetDeliveryByIdDto>.Success(
                delivery,
                _localizer["Success"],
                200);
        }
    }
}
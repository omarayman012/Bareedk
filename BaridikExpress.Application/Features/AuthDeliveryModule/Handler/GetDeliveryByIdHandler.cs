using BaridikExpress.Application.DTOs.DeliveryModule;
using BaridikExpress.Application.Features.DeliveryModule.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryModule.Handler
{
    public class GetDeliveryByIdHandler
        : IRequestHandler<GetDeliveryByIdQuery, Result<GetDeliveryByIdDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<GetDeliveryByIdHandler> _localizer;

        public GetDeliveryByIdHandler(
            IApplicationDbContext context,
            IStringLocalizer<GetDeliveryByIdHandler> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<GetDeliveryByIdDto>> Handle(
            GetDeliveryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var delivery = await _context.Deliveries
                .Include(x => x.User)
                .Where(x => x.Id == request.Id)
                .Select(x => new GetDeliveryByIdDto
                {
                    Id = x.Id,


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

                    // ADDRESS
                    Country = x.Country,
                    Gov = x.Gov,
                    City = x.City,
                    Village = x.Village,
                    Address = x.Address,
                    Floor = x.Floor,
                    Apt = x.Apt,

                    // OPTIONAL
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

using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Application.Features.PublishingHouseModule.Dto;
using BaridikExpress.Application.Features.PublishingHouseModule.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Handler
{
    public class GetByIdPublishingHouseHandler
         : IRequestHandler<GetByIdPublishingHouseQuery, Result<PublishingHouseDetailsDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public GetByIdPublishingHouseHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<PublishingHouseDetailsDto>> Handle(
     GetByIdPublishingHouseQuery request,
     CancellationToken cancellationToken)
        {
            try
            {
                var dto = await _context.PublishingHouses
                    .AsNoTracking()
                    .Where(x => x.Id == request.Id)
                    .Select(entity => new PublishingHouseDetailsDto
                    {
                        Id = entity.Id,
                        Code = entity.Code,

                        Name = new LocalizedDto
                        {
                            AR = entity.NameAr,
                            EN = entity.NameEn
                        },

                        EmailAddress = entity.EmailAddress,
                        PhoneNumber = entity.PhoneNumber,
                        WebsiteLink = entity.WebsiteLink,
                        Address = entity.Address,
                        LogoImage = entity.LogoImage,

                        // ===== Country =====
                        Country = new LocalizedNameDto
                        {
                            Id = entity.Country.CountryId,
                            AR = entity.Country.CountryNameAr,
                            EN = entity.Country.CountryNameEn
                        },

                        // ===== Government =====
                        Government = new LocalizedNameDto
                        {
                            Id = entity.Government.GovernmentId,
                            AR = entity.Government.GovernmentNameAr,
                            EN = entity.Government.GovernmentNameEn
                        },

                        // ===== City =====
                        City = entity.City == null ? null : new LocalizedNameDto
                        {
                            Id = entity.City.CityId,
                            AR = entity.City.CityNameAr,
                            EN = entity.City.CityNameEn
                        },

                        // ===== Village =====
                        Village = entity.Village == null ? null : new LocalizedNameDto
                        {
                            Id = entity.Village.VillageId,
                            AR = entity.Village.VillageNameAr,
                            EN = entity.Village.VillageNameEn
                        },

                        Street = entity.Street,
                        BuildingNumber = entity.BuildingNumber,
                        FloorNumber = entity.FloorNumber,
                        DistinctiveMark = entity.DistinctiveMark,
                        ZipCode = entity.ZipCode,
                        Active = entity.Active,

                        CreatedAt = entity.CreatedAt,
                        UpdatedAt = entity.UpdatedAt,

                        CreatedBy = _context.ApplicationUsers
                            .Where(u => u.Id == entity.CreatedById)
                            .Select(u => u.FullName)
                            .FirstOrDefault(),

                        UpdatedBy = entity.UpdatedById != null
                            ? _context.ApplicationUsers
                                .Where(u => u.Id == entity.UpdatedById)
                                .Select(u => u.FullName)
                                .FirstOrDefault()
                            : null
                    })
                    .FirstOrDefaultAsync(cancellationToken);

                if (dto == null)
                {
                    return Result<PublishingHouseDetailsDto>.Failure(
                        _localizer["NotFound"],
                        404);
                }

                return Result<PublishingHouseDetailsDto>.Success(
                    dto,
                    _localizer["Success"],
                    200);
            }
            catch (Exception ex)
            {
                return Result<PublishingHouseDetailsDto>.Error(
                    _localizer["FailedToGetData", ex.Message],
                    500);
            }
        }
    }
}
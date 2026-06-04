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
    public class GetAllPublishingHouseHandler
        : IRequestHandler<GetAllPublishingHouseQuery, Result<PaginatedList<PublishingHouseGetAllDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer _localizer;

        public GetAllPublishingHouseHandler(
            IApplicationDbContext context,
            IStringLocalizer localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<Result<PaginatedList<PublishingHouseGetAllDto>>> Handle(
            GetAllPublishingHouseQuery request,
            CancellationToken cancellationToken)
        {
            try
            {
                if (request.PageNumber <= 0 || request.PageSize <= 0)
                {
                    return Result<PaginatedList<PublishingHouseGetAllDto>>.Failure(
                        _localizer["InvalidPaginationParameters"],
                        400);
                }

                var query = _context.PublishingHouses
                    .AsNoTracking()
                    .AsQueryable();

                // ===== Filters =====
                if (!string.IsNullOrWhiteSpace(request.Name))
                {
                    var search = request.Name.Trim();
                    query = query.Where(x =>
                        x.NameAr.Contains(search) ||
                        x.NameEn.Contains(search));
                }

                if (!string.IsNullOrWhiteSpace(request.CreatedById))
                {
                    query = query.Where(x => x.CreatedById == request.CreatedById);
                }

                if (request.FromDate.HasValue)
                {
                    query = query.Where(x => x.CreatedAt >= request.FromDate.Value);
                }

                if (request.ToDate.HasValue)
                {
                    query = query.Where(x =>
                        x.CreatedAt < request.ToDate.Value.Date.AddDays(1));
                }

                query = query.OrderByDescending(x => x.CreatedAt);

                // ===== Projection =====
                var selectQuery = query.Select(x => new PublishingHouseGetAllDto
                {
                    Id = x.Id,
                    Code = x.Code,

                    Name = new LocalizedDto
                    {
                        AR = x.NameAr,
                        EN = x.NameEn
                    },

                    EmailAddress = x.EmailAddress,
                    PhoneNumber = x.PhoneNumber,
                    WebsiteLink = x.WebsiteLink,
                    Address = x.Address,

                    Country = new LocalizedNameDto
                    {
                        Id = x.Country.CountryId,
                        AR = x.Country.CountryNameAr,
                        EN = x.Country.CountryNameEn
                    },

                    Government = new LocalizedNameDto
                    {
                        Id = x.Government.GovernmentId,
                        AR = x.Government.GovernmentNameAr,
                        EN = x.Government.GovernmentNameAr
                    },

                    City = x.CityId == null ? null : new LocalizedNameDto
                    {
                        Id = x.City!.CityId,
                        AR = x.City.CityNameAr,
                        EN = x.City.CityNameEn
                    },

                    Village = x.VillageId == null ? null : new LocalizedNameDto
                    {
                        Id = x.Village!.VillageId,
                        AR = x.Village.VillageNameAr,
                        EN = x.Village.VillageNameEn
                    },

                    Street = x.Street,
                    BuildingNumber = x.BuildingNumber,
                    FloorNumber = x.FloorNumber,
                    DistinctiveMark = x.DistinctiveMark,
                    ZipCode = x.ZipCode,

                    LogoImage = x.LogoImage,

                    CreatedAt = x.CreatedAt,
                    Active = x.Active,

                    CreatedBy = _context.ApplicationUsers
                        .Where(u => u.Id == x.CreatedById)
                        .Select(u => u.FullName)
                        .FirstOrDefault()
                });

                var result = await PaginatedList<PublishingHouseGetAllDto>.CreateAsync(
                    selectQuery,
                    request.PageNumber,
                    request.PageSize);

                return Result<PaginatedList<PublishingHouseGetAllDto>>.Success(
                    result,
                    _localizer["Success"],
                    200);
            }
            catch (Exception ex)
            {
                return Result<PaginatedList<PublishingHouseGetAllDto>>.Error(
                    _localizer["FailedToRetrieveData", ex.Message],
                    500);
            }
        }
    }
}
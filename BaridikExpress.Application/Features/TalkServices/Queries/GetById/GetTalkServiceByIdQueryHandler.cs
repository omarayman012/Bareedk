using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.TalkServices.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.TalkServices.Queries.GetById;

public sealed class GetTalkServiceByIdQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetTalkServiceByIdQuery, Result<GetTalkServiceDto>>
{
    public async Task<Result<GetTalkServiceDto>> Handle(
    GetTalkServiceByIdQuery request,
    CancellationToken cancellationToken)
    {
        var target = await db.TalkServices
            .AsNoTracking()
            .Include(x => x.ServiceBusinessPlan)
            .Include(x => x.Country)
            .Include(x => x.Government)
            .Include(x => x.City)
            .Include(x => x.Village)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (target is null)
            return Result<GetTalkServiceDto>.Failure(localizer["TalkServiceNotFound"]);

        var allServices = await db.TalkServices
            .AsNoTracking()
            .Include(x => x.ServiceBusinessPlan)
            .Where(x => x.WorkEmail == target.WorkEmail)
            .Select(x => new LocalizedEntityDto
            {
                Id = x.ServiceBusinessPlan.Id,
                Name = new LocalizeLang
                {
                    AR = x.ServiceBusinessPlan.NameAr,
                    EN = x.ServiceBusinessPlan.NameEn
                }
            })
            .ToListAsync(cancellationToken);

        var dto = new GetTalkServiceDto
        {
            Id = target.Id,

            Services = allServices,

            ShipmentVolumeRange = target.ShipmentVolumeRange.ToString(),
            FirstName = target.FirstName,
            LastName = target.LastName,

            Country = new LocalizedEntityDto
            {
                Id = target.Country!.CountryId,
                Name = new LocalizeLang
                {
                    AR = target.Country.CountryNameAr,
                    EN = target.Country.CountryNameEn
                }
            },

            Government = new LocalizedEntityDto
            {
                Id = target.Government!.GovernmentId,
                Name = new LocalizeLang
                {
                    AR = target.Government.GovernmentNameAr,
                    EN = target.Government.GovernmentNameEn
                }
            },

            City = target.City == null ? null : new LocalizedEntityDto
            {
                Id = target.City.CityId,
                Name = new LocalizeLang
                {
                    AR = target.City.CityNameAr,
                    EN = target.City.CityNameEn
                }
            },

            Village = target.Village == null ? null : new LocalizedEntityDto
            {
                Id = target.Village.VillageId,
                Name = new LocalizeLang
                {
                    AR = target.Village.VillageNameAr,
                    EN = target.Village.VillageNameEn
                }
            },

            PostalCode = target.PostalCode,
            PhoneNumber = target.PhoneNumber,
            WorkEmail = target.WorkEmail,
            JobTitle = target.JobTitle,
            CompanyName = target.CompanyName,
            CompanyAddress = target.CompanyAddress,
            WebsiteUrl = target.WebsiteUrl,
            RequiredDetails = target.AdditionalInformation,
            Status = target.Status,
            CreatedAt = target.CreatedAt
        };

        return Result<GetTalkServiceDto>.Success(
            dto,
            localizer["TalkServiceRetrievedSuccessfully"]);
    }
}
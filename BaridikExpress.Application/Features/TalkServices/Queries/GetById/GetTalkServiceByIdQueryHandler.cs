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
        var talkService = await db.TalkServices
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new GetTalkServiceDto
            {
                Id = x.Id,

                Services = new List<LocalizedEntityDto>
                {
                    new LocalizedEntityDto
                    {
                        Id = x.ServiceBusinessPlan.Id,
                        Name = new LocalizeLang
                        {
                            AR = x.ServiceBusinessPlan.NameAr,
                            EN = x.ServiceBusinessPlan.NameEn
                        }
                    }
                },

                ShipmentVolumeRange = x.ShipmentVolumeRange.ToString(),

                FirstName = x.FirstName,
                LastName = x.LastName,

                Country = new LocalizedEntityDto
                {
                    Id = x.Country!.CountryId,
                    Name = new LocalizeLang
                    {
                        AR = x.Country.CountryNameAr,
                        EN = x.Country.CountryNameEn
                    }
                },

                Government = new LocalizedEntityDto
                {
                    Id = x.Government!.GovernmentId,
                    Name = new LocalizeLang
                    {
                        AR = x.Government.GovernmentNameAr,
                        EN = x.Government.GovernmentNameEn
                    }
                },

                City = x.City == null
                    ? null
                    : new LocalizedEntityDto
                    {
                        Id = x.City.CityId,
                        Name = new LocalizeLang
                        {
                            AR = x.City.CityNameAr,
                            EN = x.City.CityNameEn
                        }
                    },

                Village = x.Village == null
                    ? null
                    : new LocalizedEntityDto
                    {
                        Id = x.Village.VillageId,
                        Name = new LocalizeLang
                        {
                            AR = x.Village.VillageNameAr,
                            EN = x.Village.VillageNameEn
                        }
                    },

                PostalCode = x.PostalCode,
                PhoneNumber = x.PhoneNumber,
                WorkEmail = x.WorkEmail,
                JobTitle = x.JobTitle,
                CompanyName = x.CompanyName,
                CompanyAddress = x.CompanyAddress,
                WebsiteUrl = x.WebsiteUrl,

                RequiredDetails = x.AdditionalInformation,

                Status = x.Status,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (talkService is null)
            return Result<GetTalkServiceDto>.Failure(
                localizer["TalkServiceNotFound"]);
        return Result<GetTalkServiceDto>.Success(
            talkService,
            localizer["TalkServiceRetrievedSuccessfully"]);
    }
}
using BaridikExpress.Application.Common.Abstractions;
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.TalkServices.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.TalkServices.Queries.GetAll;

public sealed class GetAllTalkServicesQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetAllTalkServicesQuery, Result<PaginatedList<GetTalkServiceDto>>>
{
    public async Task<Result<PaginatedList<GetTalkServiceDto>>> Handle(
    GetAllTalkServicesQuery request,
    CancellationToken cancellationToken)
    {
        var query = db.TalkServices
            .AsNoTracking()
            .Include(x => x.ServiceBusinessPlan)
            .Include(x => x.Country)
            .Include(x => x.Government)
            .Include(x => x.City)
            .Include(x => x.Village)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            var name = $"%{request.Name.Trim()}%";
            query = query.Where(x =>
                EF.Functions.Like(x.FirstName, name) ||
                EF.Functions.Like(x.LastName, name) ||
                EF.Functions.Like(x.FirstName + " " + x.LastName, name) ||
                EF.Functions.Like(x.WorkEmail, name) ||
                EF.Functions.Like(x.PhoneNumber, name));
        }

        if (request.FromDate.HasValue)
            query = query.Where(x => x.CreatedAt >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(x => x.CreatedAt < request.ToDate.Value.AddDays(1));

        var rawData = await query
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);

        var grouped = rawData
            .GroupBy(x => x.WorkEmail)
            .Select(g =>
            {
                var first = g.OrderByDescending(x => x.CreatedAt).First();

                return new GetTalkServiceDto
                {
                    Id = first.Id,

                    Services = g.Select(x => new LocalizedEntityDto
                    {
                        Id = x.ServiceBusinessPlan.Id,
                        Name = new LocalizeLang
                        {
                            AR = x.ServiceBusinessPlan.NameAr,
                            EN = x.ServiceBusinessPlan.NameEn
                        }
                    }).ToList(),

                    ShipmentVolumeRange = first.ShipmentVolumeRange.ToString(),
                    FirstName = first.FirstName,
                    LastName = first.LastName,

                    Country = new LocalizedEntityDto
                    {
                        Id = first.Country!.CountryId,
                        Name = new LocalizeLang
                        {
                            AR = first.Country.CountryNameAr,
                            EN = first.Country.CountryNameEn
                        }
                    },

                    Government = new LocalizedEntityDto
                    {
                        Id = first.Government!.GovernmentId,
                        Name = new LocalizeLang
                        {
                            AR = first.Government.GovernmentNameAr,
                            EN = first.Government.GovernmentNameEn
                        }
                    },

                    City = first.City == null ? null : new LocalizedEntityDto
                    {
                        Id = first.City.CityId,
                        Name = new LocalizeLang
                        {
                            AR = first.City.CityNameAr,
                            EN = first.City.CityNameEn
                        }
                    },

                    Village = first.Village == null ? null : new LocalizedEntityDto
                    {
                        Id = first.Village.VillageId,
                        Name = new LocalizeLang
                        {
                            AR = first.Village.VillageNameAr,
                            EN = first.Village.VillageNameEn
                        }
                    },

                    PostalCode = first.PostalCode,
                    PhoneNumber = first.PhoneNumber,
                    WorkEmail = first.WorkEmail,
                    JobTitle = first.JobTitle,
                    CompanyName = first.CompanyName,
                    CompanyAddress = first.CompanyAddress,
                    WebsiteUrl = first.WebsiteUrl,
                    RequiredDetails = first.AdditionalInformation,
                    Status = first.Status,
                    CreatedAt = first.CreatedAt
                };
            })
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        var paginatedResult = PaginatedList<GetTalkServiceDto>.Create(
            grouped,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedList<GetTalkServiceDto>>.Success(
            paginatedResult,
            localizer["TalkServiceRetrievedSuccessfully"]);
    }
}
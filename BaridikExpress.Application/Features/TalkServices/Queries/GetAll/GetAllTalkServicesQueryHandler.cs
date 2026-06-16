using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Common.Helpers;
using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.TalkServices.DTOs;

namespace BaridikExpress.Application.Features.TalkServices.Queries.GetAll
{
    public sealed class GetAllTalkServicesQueryHandler(
      IApplicationDbContext db)
      : IRequestHandler<GetAllTalkServicesQuery, PaginatedList<GetTalkServiceDto>>
    {
        public async Task<PaginatedList<GetTalkServiceDto>> Handle(
            GetAllTalkServicesQuery request,
            CancellationToken cancellationToken)
        {
            var query = db.TalkServices
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = $"%{request.Search.Trim()}%";

                query = query.Where(x =>
                    EF.Functions.Like(x.FirstName, search) ||
                    EF.Functions.Like(x.LastName, search) ||
                    EF.Functions.Like(x.WorkEmail, search) ||
                    EF.Functions.Like(x.PhoneNumber, search));
            }

            if (request.FromDate.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt.Date >= request.FromDate.Value.Date);
            }

            if (request.ToDate.HasValue)
            {
                query = query.Where(x =>
                    x.CreatedAt.Date <= request.ToDate.Value.Date);
            }

            var result = query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new GetTalkServiceDto
                {
                    Id = x.Id,

                    Service = new LocalizedEntityDto
                    {
                        Id = x.ServiceBusinessPlan.Id,
                        Name = new LocalizeLang
                        {
                            AR = x.ServiceBusinessPlan.NameAr,
                            EN = x.ServiceBusinessPlan.NameEn
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
                });

            return await PaginatedList<GetTalkServiceDto>.CreateAsync(
                result,
                request.PageNumber,
                request.PageSize);
        }
    }
}

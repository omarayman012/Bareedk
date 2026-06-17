using BaridikExpress.Application.Features.TalkServices.DTOs;
using BaridikExpress.Application.Interfaces.File;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.TalkServices.Queries.Export;

public sealed class ExportTalkServicesQueryHandler(
    IApplicationDbContext db,
    IExcelService excelService)
    : IRequestHandler<ExportTalkServicesQuery, byte[]>
{
    public async Task<byte[]> Handle(
        ExportTalkServicesQuery request,
        CancellationToken cancellationToken)
    {
        var data = await db.TalkServices
            .AsNoTracking()
            .Select(x => new ExportTalkServiceResponse
            {
                Services = 
                    x.ServiceBusinessPlan.NameEn,

                ShipmentVolumeRange = x.ShipmentVolumeRange.ToString(),

                FirstName = x.FirstName,
                LastName = x.LastName,

                Country = x.Country!.NameEn??"",
                Government = x.Government!.NameEn??"",
                City = x.City != null ? x.City.NameEn : null,
                Village = x.Village != null ? x.Village.NameEn : null,

                PostalCode = x.PostalCode,
                PhoneNumber = x.PhoneNumber,
                WorkEmail = x.WorkEmail,
                JobTitle = x.JobTitle,
                CompanyName = x.CompanyName,
                CompanyAddress = x.CompanyAddress,
                WebsiteUrl = x.WebsiteUrl,
                RequiredDetails = x.AdditionalInformation,

                Status = x.Status.ToString(),

                CreatedAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return await excelService.DownloadDataAsync(data);
    }
}
using BaridikExpress.Application.Features.Announcements.DTO;
using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Announcementes;

namespace BaridikExpress.Application.Features.Announcements.Commands.UploadAnnouncements;

public class UploadAnnouncementsCommandHandler(
    IExcelService excelService,
    IApplicationDbContext context)
    : IRequestHandler<
        UploadAnnouncementsCommand,
        Result<ExcelUploadResult<Announcement>>>
{
    public async Task<Result<ExcelUploadResult<Announcement>>> Handle(
        UploadAnnouncementsCommand request,
        CancellationToken cancellationToken)
    {
        var result = await excelService
            .UploadAsync<AnnouncementExcelDto, Announcement>(
                request.File,

                dto => new Announcement(
                    dto.TitleEn,
                    dto.TitleAr,
                    dto.BackgroundColor,
                    dto.TextColor),

                async entity => await context.Announcements
                    .AsNoTracking()
                    .AnyAsync(x =>
                        x.TitleAr == entity.TitleAr ||
                        x.TitleEn == entity.TitleEn,
                        cancellationToken),

                entity => $"{entity.TitleAr}|{entity.TitleEn}",
                cancellationToken);

        return Result<ExcelUploadResult<Announcement>>
            .Success(result);
    }
}

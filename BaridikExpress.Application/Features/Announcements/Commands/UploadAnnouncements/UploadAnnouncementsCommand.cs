using BaridikExpress.Application.Interfaces.File;
using BaridikExpress.Domain.Entities.Announcementes;

namespace BaridikExpress.Application.Features.Announcements.Commands.UploadAnnouncements
{
    public record UploadAnnouncementsCommand(IFormFile File) 
        : IRequest<Result<ExcelUploadResult<Announcement>>>;
}

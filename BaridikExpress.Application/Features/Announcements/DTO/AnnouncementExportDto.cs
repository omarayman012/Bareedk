
namespace BaridikExpress.Application.Features.Announcements.DTO
{
    public sealed class AnnouncementExportDto
    {
        public string TitleAr { get; set; } = default!;
        public string TitleEn { get; set; } = default!;

        public string TextColor { get; set; } = default!;
        public string BackgroundColor { get; set; } = default!;
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; }
    }
}

using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Announcements.DTO
{
    public sealed class GetAllAnnouncementsMobileDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Title { get; set; } = default!;
        public string BackgroundColor { get; set; } = default!;
        public string TextColor { get; set; } = default!;
        public bool IsActive { get; set; }
    }
}

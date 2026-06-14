using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Announcements.DTO
{
    public sealed class GetAllAnnouncementsDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Title { get; set; } = default!;
        public string BackgroundColor { get; set; } = string.Empty;
        public string TextColor { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}

namespace BaridikExpress.Application.Features.Announcements.DTO
{
    public class AnnouncementExcelDto
    {
        public string TitleAr { get; set; } = default!;

        public string TitleEn { get; set; } = default!;

        public string? DescriptionEn { get; set; } = default!;
        public string? DescriptionAr { get; set; } = default!;
         public string? Discount { get; set; }

        public string TextColor { get; set; } = default!;

        public string BackgroundColor { get; set; } = default!;
    }
}

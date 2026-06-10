
namespace BaridikExpress.Application.Features.Banners.DTO
{
    public sealed class BannerExportDto
    {
        public string TitleAr { get; set; } = default!;
        public string TitleEn { get; set; } = default!;

        public string DescriptionAr { get; set; } = default!;
        public string DescriptionEn { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = default!;
        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; } = default!;
        public DateTime? UpdatedAt { get; set; }
    }
}

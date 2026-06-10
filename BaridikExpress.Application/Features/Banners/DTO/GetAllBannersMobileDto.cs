using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Banners.DTO
{
    public sealed class GetAllBannersMobileDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Title { get; set; } = default!;
        public LocalizedDto Description { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
    }
}

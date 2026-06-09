using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.Banners.DTO
{
    public sealed class GetAllBannersDto
    {
        public Guid Id { get; set; }

        public LocalizedDto Title { get; set; } = default!;

        public LocalizedDto Description { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
    }
}

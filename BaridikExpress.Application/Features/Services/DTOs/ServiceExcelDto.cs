using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.Services.DTOs
{
    public sealed class ServiceExcelDto
    {
        public string NameEn { get; set; } = string.Empty;

        public string NameAr { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public Currency Currency { get; set; }

        public string? ImageUrl { get; set; }
    }
}

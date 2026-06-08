

using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class ResponseBlogsAuthorDto
    {
        public Guid Id { get; set; }

        public LocalizedDto Name { get; set; } = new();

        public string Gender { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public LocalizedNameDto BlogsCategoryName { get; set; }

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } = string.Empty;
    }
}
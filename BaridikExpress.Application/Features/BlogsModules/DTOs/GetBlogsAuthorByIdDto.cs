
using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class GetBlogsAuthorByIdDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Name { get; set; } = new();
        public string Gender { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public LocalizedNameDto Category { get; set; } = new();
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public int BlogsCount { get; set; }
        public List<AuthorBlogDto> Blogs { get; set; } = new();
    }

    public class AuthorBlogDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Title { get; set; } = new();
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class GetBlogsCategoryByIdDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Name { get; set; } = new();
        public int? Priorty { get; set; }
        public LocalizedDto Description { get; set; } = new();
        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int BlogsCount { get; set; }
        public List<BlogBasicDto> Blogs { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
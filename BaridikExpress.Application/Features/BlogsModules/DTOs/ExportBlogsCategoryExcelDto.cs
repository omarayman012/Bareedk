namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class ExportBlogsCategoryExcelDto
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public int? Priorty { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public bool IsActive { get; set; }
        public int BlogsCount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
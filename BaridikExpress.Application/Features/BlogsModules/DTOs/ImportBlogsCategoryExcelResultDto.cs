using System.Collections.Generic;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class ImportBlogsCategoryExcelResultDto
    {
        public int TotalRows { get; set; }
        public int UploadedCount { get; set; }
        public int SkippedCount { get; set; }
        public List<int> SkippedRows { get; set; } = new();
        public List<string> Messages { get; set; } = new();
    }
}
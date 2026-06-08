using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class BlogsCategoryDto
    {
        public Guid Id { get; set; }

        public string? NameAr { get; set; } = string.Empty;

        public string? NameEn { get; set; } = string.Empty;

        public int? Priorty { get; set; }

        public string DescriptionAr { get; set; } = string.Empty;

        public string DescriptionEn { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; } 

    }
}

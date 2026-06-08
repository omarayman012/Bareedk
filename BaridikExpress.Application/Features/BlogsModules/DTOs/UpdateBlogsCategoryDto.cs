using BaridikExpress.Application.Features.CommanDTO.Localizes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class UpdateBlogsCategoryDto
    {
        public Guid Id { get; set; }
        public LocalizedDto Name { get; set; }
        public int? Priorty { get; set; }
        public LocalizedDto Description { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string UpdatedBy { get; set; }
    }
}

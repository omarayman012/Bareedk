using BaridikExpress.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.BlogsModules
{
    public class BlogsCategory : BaseEntity
    {
        public Guid Id { get; set; }

        public string? NameAr { get; set; } = string.Empty;

        public string? NameEn { get; set; } = string.Empty;

        public int? Priorty { get; set; }

        public string? DescriptionAr { get; set; } = string.Empty;

        public string? DescriptionEn { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public bool IsActive { get; set; }
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    }
}

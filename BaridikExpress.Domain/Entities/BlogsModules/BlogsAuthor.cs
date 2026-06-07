using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.BlogsModules
{
    public class BlogsAuthor : BaseEntity
    {
        public Guid Id { get; set; }

        public string? NameAr { get; set; } = string.Empty;
        public string? NameEn { get; set; } = string.Empty;

        public UserGender Gender { get; set; }

        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public Guid BlogsCategoryId { get; set; }
        public BlogsCategory BlogsCategory { get; set; } = null!;

        public bool IsActive { get; set; }
        public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    }
}

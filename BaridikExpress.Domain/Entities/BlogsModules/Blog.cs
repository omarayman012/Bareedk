using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.PublishingHouseModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.BlogsModules
{
    public class Blog : BaseEntity
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public Guid BlogsCategoryId { get; set; }
        public Guid BlogsAuthorId { get; set; }
        public Guid? PublishingHouseId { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public TimeOnly? CreatedTime { get; set; }

        // Navigation
        public BlogsCategory Category { get; set; } = default!;
        public BlogsAuthor Author { get; set; }
        public PublishingHouse? PublishingHouse { get; set; }
        public BlogSeo? Seo { get; set; }
        public ICollection<BlogTag> BlogTags { get; set; } = new List<BlogTag>();
        public ICollection<BlogReaction> Reactions { get; set; } = new List<BlogReaction>(); 
        public ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();
    }
}

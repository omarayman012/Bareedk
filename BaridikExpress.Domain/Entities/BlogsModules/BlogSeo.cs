using BaridikExpress.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.BlogsModules
{

    public class BlogSeo : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid BlogId { get; set; }
        public string? MetaTitleAr { get; set; }
        public string? MetaTitleEn { get; set; }
        public string? SlugAr { get; set; }
        public string? SlugEn { get; set; }
        public string? MetaKeywordsAr { get; set; }
        public string? MetaKeywordsEn { get; set; }
        public string? MetaImage { get; set; }
        public string? MetaDescriptionAr { get; set; }
        public string? MetaDescriptionEn { get; set; }

        public Blog? Blog { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.BlogsModules.DTOs
{
    public class FavoriteBlogDto
    {
        public Guid Id { get; set; }

        public NameDto Title { get; set; }

        public DescriptionDto Description { get; set; }

        public string Image { get; set; }

        public LookupDto Category { get; set; }

        public LookupDto Author { get; set; }

        public DateOnly? CreatedDate { get; set; }

        public TimeOnly? CreatedTime { get; set; }

        public int CommentsCount { get; set; }

        public int ReactionsCount { get; set; }
    }
}

using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.BlogsModules
{
    public class BlogComment : BaseEntity
    {
        public Guid Id { get; set; }

        public Guid BlogId { get; set; }
        public Blog Blog { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public string Content { get; set; }

        public Guid? ParentId { get; set; }
        public BlogComment? Parent { get; set; }
        public ICollection<BlogComment> Replies { get; set; } = new List<BlogComment>();

        public ICollection<CommentReaction> Reactions { get; set; } = new List<CommentReaction>();
    }
}

using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.BlogsModules
{
    public class CommentReaction
    {
        public Guid Id { get; set; }

        public Guid CommentId { get; set; }
        public BlogComment Comment { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ReactionType Type { get; set; }
    }
}

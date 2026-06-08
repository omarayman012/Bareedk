using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.BlogsModules
{
    public class BlogReaction : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid BlogId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ReactionType Type { get; set; }

        public Blog Blog { get; set; }
    }
}

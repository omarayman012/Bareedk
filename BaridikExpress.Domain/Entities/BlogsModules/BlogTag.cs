using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.BlogsModules
{

    public class BlogTag
    {
        public Guid BlogId { get; set; }
        public Guid TagId { get; set; }

        public Blog? Blog { get; set; }
        public Tag? Tag { get; set; }
    }

}

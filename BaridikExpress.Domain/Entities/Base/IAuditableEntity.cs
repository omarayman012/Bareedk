using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.Base
{
    public interface IAuditableEntity
    {
        string? CreatedById { get; set; }
        DateTime CreatedAt { get; set; }

        string? UpdatedById { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}

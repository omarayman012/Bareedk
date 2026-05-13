using BaridikExpress.Domain.Entities.AuthModules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.Base
{
    public class BaseEntity: IAuditableEntity
    {
        public string? CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? UpdatedById { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public User? CreatedBy { get; set; }
        public User? UpdatedBy { get; set; }
    }
}

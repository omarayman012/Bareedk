using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.CareerFields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.ClientModule
{
    public class Client : BaseEntity
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        public Guid CareerFieldId { get; set; }
        public CareerField CareerField { get; set; } = null!;

        public string? CompanyName { get; set; }
        public string? CompanyLink { get; set; }

        public bool AcceptTerms { get; set; }
        public bool AcceptPrivacy { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Nationality
{
    public class Nationality : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

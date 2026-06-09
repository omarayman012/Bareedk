using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.Base
{
    public interface ISelectMenuEntity
    {
        Guid Id { get; }
        string? NameAr { get; }
        string? NameEn { get; }
        Guid? ParentId { get; }
    }
}

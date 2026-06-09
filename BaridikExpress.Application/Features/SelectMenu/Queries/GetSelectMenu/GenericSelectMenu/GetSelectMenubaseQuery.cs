using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.GenericSelectMenu
{
    public class GetSelectMenubaseQuery<T> : IRequest<Result<IEnumerable<SelectMenuResponse>>>
    where T : BaseEntity, ISelectMenuEntity
    {
        public Guid? ParentId { get; init; }
        public string? Name { get; init; }
    }
}

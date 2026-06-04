using BaridikExpress.Application.DTOs;
using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Domain.Common;
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Application.Features.SelectMenu.Queries.GetSelectMenu.Location;

public sealed class GetSelectMenuQuery<T> : IRequest<Result<IEnumerable<SelectMenuResponse>>>
    where T : BaseEntity, ISelectMenuEntity
{
    public Guid? ParentId { get; init; }
}
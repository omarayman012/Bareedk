using BaridikExpress.Application.Features.SelectMenu.DTOs;
using BaridikExpress.Domain.Entities.Base;

public sealed class GetSelectMenuQuery<T> : IRequest<Result<IEnumerable<SelectMenuResponse>>>
    where T : BaseEntity, ISelectMenuEntity
{
    public Guid? ParentId { get; init; }
    public string? Name { get; init; } 
}
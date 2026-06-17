using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.OurPlans.DTO;

public sealed class GetAllPlansDto
{
    public Guid Id { get; set; }

    public LocalizedDto Name { get; set; } = default!;

    public PlanType Type { get; set; }

    public LocalizedDto? Description { get; set; }

    public LocalizedListDto Features { get; set; } = new LocalizedListDto();

    public bool IsActive { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
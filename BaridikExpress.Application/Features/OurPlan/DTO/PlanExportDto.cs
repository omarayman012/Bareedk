namespace BaridikExpress.Application.Features.OurPlans.DTO;

public sealed class PlanExportDto
{
    public string NameAr { get; set; } = default!;

    public string NameEn { get; set; } = default!;

    public string Type { get; set; } = default!;

    public string? DescriptionAr { get; set; }

    public string? DescriptionEn { get; set; }

    public string FeaturesAr { get; set; } = default!;

    public string FeaturesEn { get; set; } = default!;

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; } = default!;

    public DateTime CreatedAt { get; set; }

    public string UpdatedBy { get; set; } = default!;

    public DateTime? UpdatedAt { get; set; }
}
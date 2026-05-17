namespace BaridikExpress.Application.Features.CareerFields.DTOs;

public class GetAllCareerFieldsDto
{
    public Guid Id { get; set; }

    public string NameAr { get; set; }

    public string NameEn { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsActive { get; set; }
}

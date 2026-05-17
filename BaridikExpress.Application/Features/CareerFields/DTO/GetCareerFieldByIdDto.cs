namespace BaridikExpress.Application.Features.CareerFields.DTO;

public class GetCareerFieldByIdDto
{
    public Guid Id { get; set; }

    public string NameAr { get; set; }

    public string NameEn { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int CustomersCount { get; set; }

    public List<CareerFieldCustomerDto> Customers { get; set; }
        = [];
}
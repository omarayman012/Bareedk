namespace BaridikExpress.Application.Features.CareerFields.DTO;

public class CareerFieldCustomerDto
{
    public Guid Id { get; set; }

    public string NameAr { get; set; }

    public string NameEn { get; set; }

    public string Email { get; set; }

    public string Mobile { get; set; }
    public string WhatsappNumber { get; set; }

    public string Address { get; set; }

    public int TotalShipments { get; set; }

    public decimal TotalSpent { get; set; }

    public bool IsActive { get; set; }
}

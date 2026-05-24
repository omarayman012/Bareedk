using BaridikExpress.Application.Features.Customer.Dtos;

namespace BaridikExpress.Application.Features.CareerFields.DTO;

public class CareerFieldCustomerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public List<CustomerContactResponse> Contacts { get; set; } = [];

    public List<CustomerAddressResponse> Addresses { get; set; } = [];

    public int TotalShipments { get; set; }

    public decimal TotalSpent { get; set; }

    public bool IsActive { get; set; }
}

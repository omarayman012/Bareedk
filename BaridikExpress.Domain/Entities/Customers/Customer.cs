using BaridikExpress.Domain.Entities.AuthModules;
using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.CareerFields;
using BaridikExpress.Domain.Entities.OurPlans;

namespace BaridikExpress.Domain.Entities.Customers;

public class Customer : BaseEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public Guid? NationalityId { get; private set; }
    public Nationality.Nationality? Nationality { get; private set; }
    public Guid? CareerFieldId { get; private set; }
    public CareerField? CareerField { get; private set; }
    public string UserId { get; private set; } = default!;
    public User User { get; private set; } = default!;

    public Guid? PlanId { get; private set; }
    public  Plan? Plan { get; private set; }

    public string? ImageUrl { get; private set; }
    public ICollection<CustomerContact> Contacts { get; private set; } = [];
    public ICollection<CustomerAddress> Addresses { get; private set; } = [];
    public CustomerAccount? Account { get; private set; }

    private Customer() { }

    public static Customer Create(
        string name,
        string userId,
        Guid? nationalityId = null,
        Guid? careerFieldId = null,
        string? imageUrl = null)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Name = name,
            UserId = userId,
            NationalityId = nationalityId,
            CareerFieldId = careerFieldId,
            ImageUrl = imageUrl,
        };
    }

    public void Update(
        string name,
        Guid? nationalityId = null,
        Guid? careerFieldId = null,
        string? imageUrl = null)
    {
        Name = name;
        NationalityId = nationalityId;
        CareerFieldId = careerFieldId;
        ImageUrl = imageUrl;
    }
}
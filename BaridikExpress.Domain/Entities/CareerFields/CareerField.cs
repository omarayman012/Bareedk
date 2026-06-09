using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Customers;
using BaridikExpress.Domain.Entities.ValueObjects;

namespace BaridikExpress.Domain.Entities.CareerFields
{
    public class CareerField : BaseEntity, ISelectMenuEntity
    {
        public Guid Id { get; private set; }
        public LocalizedString Name { get; private set; } = default!;
        public ICollection<Customer> Customers { get; private set; }
            = new List<Customer>();

        #region ISelectMenuEntity Implementation Mapping
        public string? NameAr =>Name.Ar;

        public string? NameEn => Name.En;

        public Guid? ParentId => null;
        #endregion

        private CareerField() { }
        public CareerField(
            string? nameEn,
            string? nameAr)
        {
            Id = Guid.NewGuid();
            SetName(nameEn, nameAr);
        }
        public void Update(
            string? nameEn,
            string? nameAr)
        {
            SetName(nameEn, nameAr);
        }
        private void SetName(
            string? nameEn,
            string? nameAr)
        {
            Name = LocalizedString.Create(nameEn, nameAr);
        }


    }
}
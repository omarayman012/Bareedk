using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Domain.Entities.Vehicles
{
    public class Vehicle : BaseEntity, ISelectMenuEntity
    {
        public Guid Id { get; private set; }
        public string NameEn { get; private set; } = default!;
        public string NameAr { get; private set; } = default!;
        public decimal LoadCapacityFrom { get; private set; }
        public decimal LoadCapacityTo { get; private set; }
        public decimal PricePerTon { get; private set; }
        public Currency Currency { get; private set; }
        public decimal? TotalPrice
            => IsPriceCalculationEnabled
                ? PricePerTon * LoadCapacityTo
                : 0;
        public string ImageUrl { get; private set; }
        public bool IsPriceCalculationEnabled { get; private set; }

        #region ISelectMenuEntity Implementation Mapping
        public Guid? ParentId => null;
        #endregion

        private Vehicle()
        {
        }

        public Vehicle(
            string nameEn,
            string nameAr,
            decimal loadCapacityFrom,
            decimal loadCapacityTo,
            decimal pricePerTon,
             Currency currency,
            string imageUrl,
            bool isPriceCalculationEnabled)
        {
            Id = Guid.NewGuid();
            SetName(nameEn, nameAr);
            SetLoadCapacity(
                loadCapacityFrom,
                loadCapacityTo);
            SetPricePerTon(pricePerTon);
            Currency = currency;
            ImageUrl = imageUrl;
            IsPriceCalculationEnabled = isPriceCalculationEnabled;
        }

        public void Update(
            string ?nameEn,
            string ?nameAr,
            decimal ?loadCapacityFrom,
            decimal ?loadCapacityTo,
            decimal ?pricePerTon,
            Currency ?currency,
            string ?imageUrl,
            bool ?isPriceCalculationEnabled)
        {
            var resolvedNameEn = nameEn ?? NameEn;
            var resolvedNameAr = nameAr ?? NameAr;
            SetName(resolvedNameEn, resolvedNameAr);

            var resolvedLoadFrom = loadCapacityFrom ?? LoadCapacityFrom;
            var resolvedLoadTo = loadCapacityTo ?? LoadCapacityTo;
            SetLoadCapacity(resolvedLoadFrom, resolvedLoadTo);
            SetPricePerTon(pricePerTon ?? PricePerTon);
            Currency = currency??Currency;
            ImageUrl = imageUrl ?? ImageUrl;
            IsPriceCalculationEnabled = isPriceCalculationEnabled ?? IsPriceCalculationEnabled;
        }

        private void SetName(
            string nameEn,
            string nameAr)
        {
            NameEn = nameEn.Trim();

            NameAr = nameAr.Trim();
        }

        private void SetLoadCapacity(
            decimal loadCapacityFrom,
            decimal loadCapacityTo)
        {
            LoadCapacityFrom = loadCapacityFrom;
            LoadCapacityTo = loadCapacityTo;
        }

        private void SetPricePerTon(decimal pricePerTon)
        {
            PricePerTon = pricePerTon;
        }
    }
}

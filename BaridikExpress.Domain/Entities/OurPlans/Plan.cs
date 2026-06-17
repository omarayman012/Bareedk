using BaridikExpress.Domain.Entities.Base;
using BaridikExpress.Domain.Entities.Customers;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.OurPlans
{
        public class Plan : BaseEntity, ISelectMenuEntity
        {
            public Guid Id { get; private set; }

            public string NameEn { get; private set; } = default!;
            public string NameAr { get; private set; } = default!;
            public PlanType Type { get; private set; } = default!;
            public List<string> FeaturesEn { get; private set; } = [];
            public List<string> FeaturesAr { get; private set; } = [];

            public string? DescriptionEn { get; private set; }
            public string? DescriptionAr { get; private set; }

        public ICollection<Customer> Customers { get; private set; }
    = new List<Customer>();

        #region ISelectMenuEntity Implementation

        string? ISelectMenuEntity.NameEn => NameEn;
            string? ISelectMenuEntity.NameAr => NameAr;
            public Guid? ParentId => null;

            #endregion

            private Plan() { }

            public Plan(
                string nameEn,
                string nameAr,
                PlanType type,
                List<string>? featuresEn,
                List<string>? featuresAr,
                string? descriptionEn,
                string? descriptionAr)
            {
                Id = Guid.NewGuid();
                NameEn = nameEn;
                NameAr = nameAr;
                Type = type; 
                FeaturesEn = featuresEn ?? [];
                FeaturesAr = featuresAr ?? [];
                DescriptionEn = descriptionEn;
                DescriptionAr = descriptionAr;
            }

            public void Update(
                string? nameEn,
                string? nameAr,
                PlanType? type,
                List<string>? featuresEn,
                List<string>? featuresAr,
                string? descriptionEn,
                string? descriptionAr)
            {
                NameEn = nameEn ?? NameEn;
                NameAr = nameAr ?? NameAr;
                Type = type ?? Type;
                FeaturesEn = featuresEn ?? FeaturesEn;
                FeaturesAr = featuresAr ?? FeaturesAr;
            DescriptionEn = descriptionEn ?? DescriptionEn;
                DescriptionAr = descriptionAr ?? DescriptionAr;
            }
        }
    }


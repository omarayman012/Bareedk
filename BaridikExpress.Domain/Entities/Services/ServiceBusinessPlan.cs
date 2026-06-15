using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.ServiceModules
{
    public class ServiceBusinessPlan : BaseEntity
    {
        public Guid Id { get; private set; }
        public string NameEn { get; private set; } = null!;
        public string NameAr { get; private set; } = null!;
        public string? DescriptionEn { get; private set; }
        public string? DescriptionAr { get; private set; }
        public string? ImageUrl { get; private set; }

        private ServiceBusinessPlan() { }

        public static ServiceBusinessPlan Create(
            string nameEn,
            string nameAr,
            string? descriptionEn,
            string? descriptionAr,
            string? imageUrl)
        {
            return new ServiceBusinessPlan
            {
                Id = Guid.NewGuid(),
                NameEn = nameEn,
                NameAr = nameAr,
                DescriptionEn = descriptionEn,
                DescriptionAr = descriptionAr,
                ImageUrl = imageUrl
            };
        }

        public void Update(
            string? nameEn,
            string? nameAr,
            string? descriptionEn,
            string? descriptionAr,
            string? imageUrl)
        {
            if (nameEn is not null) NameEn = nameEn;
            if (nameAr is not null) NameAr = nameAr;
            if (descriptionEn is not null) DescriptionEn = descriptionEn;
            if (descriptionAr is not null) DescriptionAr = descriptionAr;
            if (imageUrl is not null) ImageUrl = imageUrl;
        }
    }
}
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.Banners
{
    public class Banner : BaseEntity
    {
        public Guid Id { get; private set; }
        public string TitleEn { get;private set; } = default!;
        public string TitleAr { get; private set; } = default!;

        public string DescriptionEn { get; private set; } = default!;
        public string DescriptionAr { get; private set; } = default!;

        public string ImageUrl { get; private set; } = default!;
        private Banner(){}
        public Banner(
            string titleEn,
            string titleAr,
            string descriptionEn,
            string descriptionAr,
            string imageUrl)
        {
            TitleEn = titleEn;
            TitleAr = titleAr;
            DescriptionEn = descriptionEn;
            DescriptionAr = descriptionAr;
            ImageUrl = imageUrl;
        }

        public void Update(
            string? titleEn,
            string? titleAr,
            string? descriptionEn,
            string? descriptionAr,
            string? imageUrl)
        {
            TitleEn = titleEn ?? TitleEn;
            TitleAr = titleAr ?? TitleAr;
            DescriptionEn = descriptionEn ?? DescriptionEn;
            DescriptionAr = descriptionAr ?? DescriptionAr;
            ImageUrl = imageUrl ?? ImageUrl;
        }
    }


}

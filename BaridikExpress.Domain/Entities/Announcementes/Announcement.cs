using BaridikExpress.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Entities.Announcementes
{
        public class Announcement : BaseEntity, ISelectMenuEntity
        {
            public Guid Id { get; private set; }

            public string TitleEn { get; private set; } = default!;
            public string TitleAr { get; private set; } = default!;
            public string? DescriptionEn { get; private set; } = default!;
            public string? DescriptionAr { get; private set; } = default!;
           public string? Discount { get; private set; }
              public string TextColor { get; private set; } = default!;
            public string BackgroundColor { get; private set; } = default!;

            #region ISelectMenuEntity Implementation

            public string? NameAr => TitleAr;
            public string? NameEn => TitleEn;
            public Guid? ParentId => null;
            #endregion

            private Announcement() { }

            public Announcement(
                string titleEn,
                string titleAr,
                string? descriptionEn,
                string? descriptionAr,
                string? discount,
                string textColor,
                string backgroundColor)
            {
                TitleEn = titleEn;
                TitleAr = titleAr;
            DescriptionEn = descriptionEn;
            DescriptionAr = descriptionAr;
            Discount = discount;
            TextColor = textColor;
                BackgroundColor = backgroundColor;
            }

            public void Update(
                string? titleEn,
                string? titleAr,
                string? descriptionEn,
                string? descriptionAr,
                string? discount,
                string? textColor,
                string? backgroundColor)
            {
                TitleEn = titleEn ?? TitleEn;
                TitleAr = titleAr ?? TitleAr;
                DescriptionEn = descriptionEn ?? DescriptionEn;
            DescriptionAr = descriptionAr ?? DescriptionAr;
                    Discount = discount ?? Discount;
            TextColor = textColor ?? TextColor;
                BackgroundColor = backgroundColor ?? BackgroundColor;
            }

        }
    
}

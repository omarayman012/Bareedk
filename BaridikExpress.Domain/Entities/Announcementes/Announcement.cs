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
                string textColor,
                string backgroundColor)
            {
                TitleEn = titleEn;
                TitleAr = titleAr;
                TextColor = textColor;
                BackgroundColor = backgroundColor;
            }

            public void Update(
                string? titleEn,
                string? titleAr,
                string? textColor,
                string? backgroundColor)
            {
                TitleEn = titleEn ?? TitleEn;
                TitleAr = titleAr ?? TitleAr;
                TextColor = textColor ?? TextColor;
                BackgroundColor = backgroundColor ?? BackgroundColor;
            }

        }
    
}

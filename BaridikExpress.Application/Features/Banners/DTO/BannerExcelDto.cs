using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Banners.DTO
{
    public class BannerExcelDto
    {
        public string TitleAr { get; set; } = default!;

        public string TitleEn { get; set; } = default!;

        public string DescriptionAr { get; set; } = default!;

        public string DescriptionEn { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }
}

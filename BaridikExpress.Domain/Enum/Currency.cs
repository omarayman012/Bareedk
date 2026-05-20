using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Domain.Enum
{
    public enum Currency
    {
        [Display(Name = "UAE Dirham")]
        UAEDirham = 1,

        [Display(Name = "American Dollar")]
        AmericanDollar = 2,

        [Display(Name = "British Pound")]
        BritishPound = 3,

        [Display(Name = "Euro")]
        Euro = 4
    }
}

using System.ComponentModel.DataAnnotations;

namespace BaridikExpress.Domain.Enum
{
    public enum Currency
    {
        [Display(Name = "SAR")]
        SAR = 1,

        [Display(Name = "USD")]
        USD = 2
    }
}
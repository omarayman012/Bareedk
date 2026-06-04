using BaridikExpress.Application.Features.CommanDTO.Localizes;
using BaridikExpress.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Common.Extensions
{
    public static class CurrencyExtensions
    {
        public static LocalizedDto ToLocalizedDto(
            this Currency currency)
        {
            return currency switch
            {
                Currency.SAR => new LocalizedDto
                {
                    EN = "SAR",
                    AR = "ريال سعودي"
                },

                Currency.USD => new LocalizedDto
                {
                    EN = "USD",
                    AR = "دولار أمريكي"
                },

                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}

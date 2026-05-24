using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.DeliveryTypes.DTO
{
    public record DeliveryTypeResponse(
       Guid Id,
       string NameEn,
       string NameAr,
       int DaysFrom,
       int DaysTo,
       decimal PricePerShipment,
       decimal PricePerTotal,
       bool IsSwitchActive,
       string? ImageUrl,
       string? DescriptionEn,
       string? DescriptionAr,
       string? CreatedBy,
       DateTime CreatedAt,
       string? UpdatedBy,
       DateTime UpdatedAt
   );
}

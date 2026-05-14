using BaridikExpress.Application.Features.CareerFields.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.CareerFields.Commands.CreateCareerFields
{
    public record CreateCareerFieldCommand(
            string NameAr ,
            string NameEn 
   ) : IRequest<Result<Guid>>;
}

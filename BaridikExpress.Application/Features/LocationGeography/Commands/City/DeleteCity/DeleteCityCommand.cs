using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.DeleteCity
{
    public class DeleteCityCommand:IRequest<Result<bool>>
    {
        public List<Guid> Ids { get; set; } = [];

    }
}

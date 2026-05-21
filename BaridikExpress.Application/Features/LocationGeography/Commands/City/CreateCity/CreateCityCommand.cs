using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.LocationGeography.Dto.City;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.City.CreateCity
{
    public class CreateCityCommand:IRequest<Result<CityDto>>
    {
        public string NameAr { get; set; }=string.Empty;
        public string NameEn { get; set; }=string.Empty;
        public Guid GovernmentId { get; set; }
    }
}

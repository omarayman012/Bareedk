using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.Features.CommanDTO.Localizes;

namespace BaridikExpress.Application.Features.LocationGeography.Dto.Village
{
    public class CreateVillageResponse
    {
        public Guid Id { get; set; }
        public LocalizedDto? Name { get; set; }
        public LocalizedNameDto? CityName { get; set; }
        public LocalizedNameDto? GovernmentName { get; set; }
        public LocalizedNameDto? CountryName { get; set; }
    }
}

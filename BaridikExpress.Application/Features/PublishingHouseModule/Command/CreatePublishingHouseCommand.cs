using BaridikExpress.Application.Features.PublishingHouseModule.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Command
{
    public class CreatePublishingHouseCommand : IRequest<Result<CreatePublishingHouseResponseDto>>
    {
        public string Code { get; set; }

        public string NameAr { get; set; }

        public string NameEn { get; set; }

        public string? EmailAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public string? WebsiteLink { get; set; }

        public string? Address { get; set; }

        // Required
        public Guid CountryId { get; set; }

        // Required
        public Guid GovernmentId { get; set; }

        public Guid? CityId { get; set; }

        public Guid? VillageId { get; set; }

        public string? Street { get; set; }

        public string? BuildingNumber { get; set; }

        public string? FloorNumber { get; set; }

        public string? DistinctiveMark { get; set; }

        public string? ZipCode { get; set; }

        // Required
        public IFormFile? Image { get; set; }
    }

}
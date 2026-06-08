using BaridikExpress.Application.Features.LocationGeography.Dto.Village;

namespace BaridikExpress.Application.Features.LocationGeography.Commands.Villiage.CreateVillage
{
    public class CreateVillageCommand : IRequest<Result<CreateVillageResponse>>
    {
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public Guid GovernmentId { get; set; }
        public Guid CountryId { get; set; }
    }
}
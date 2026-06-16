using BaridikExpress.Application.Common.Models;
using BaridikExpress.Domain.Enum;

namespace BaridikExpress.Application.Features.TalkServices.Commands.Create;

public sealed record CreateTalkServiceCommand(
    List<Guid> ServiceBusinessPlanIds,
    ShipmentVolumeRange ShipmentVolumeRange,
    string FirstName,
    string LastName,
    Guid CountryId,
    Guid GovernmentId,
    Guid? CityId,
    Guid? VillageId,
    string PostalCode,
    string PhoneNumber,
    string WorkEmail,
    string JobTitle,
    string CompanyName,
    string CompanyAddress,
    string WebsiteUrl,
    string AdditionalInformation
) : IRequest<Result<Guid>>;
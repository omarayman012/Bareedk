using BaridikExpress.Application.Common.Models;
using BaridikExpress.Domain.Entities.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BaridikExpress.Application.Features.TalkServices.Commands.Create;

public sealed class CreateTalkServiceCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<CreateTalkServiceCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(
        CreateTalkServiceCommand request,
        CancellationToken cancellationToken)
    {
        var alreadyExists = await db.TalkServices
            .AnyAsync(x =>
                x.WorkEmail == request.WorkEmail &&
                request.ServiceBusinessPlanIds.Contains(x.ServiceBusinessPlanId),
            cancellationToken);

        if (alreadyExists)
            return Result<Guid>.Failure(localizer["TalkServiceAlreadyExists"]);

        var existingPlanIds = await db.ServiceBusinessPlans
            .Where(x => request.ServiceBusinessPlanIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        var missingPlanIds = request.ServiceBusinessPlanIds
            .Except(existingPlanIds)
            .ToList();

        if (missingPlanIds.Count != 0)
            return Result<Guid>.Failure(localizer["ServiceBusinessPlanNotFound"]);

        var countryExists = await db.Countries
            .AnyAsync(x => x.CountryId == request.CountryId, cancellationToken);

        if (!countryExists)
            return Result<Guid>.Failure(localizer["CountryNotFound"]);

        var governmentExists = await db.Governments
            .AnyAsync(x => x.GovernmentId == request.GovernmentId, cancellationToken);

        if (!governmentExists)
            return Result<Guid>.Failure(localizer["GovernmentNotFound"]);

        if (request.CityId.HasValue)
        {
            var cityExists = await db.Cities
                .AnyAsync(x => x.CityId == request.CityId.Value, cancellationToken);

            if (!cityExists)
                return Result<Guid>.Failure(localizer["CityNotFound"]);
        }

        if (request.VillageId.HasValue)
        {
            var villageExists = await db.Villages
                .AnyAsync(x => x.VillageId == request.VillageId.Value, cancellationToken);

            if (!villageExists)
                return Result<Guid>.Failure(localizer["VillageNotFound"]);
        }

        var entities = request.ServiceBusinessPlanIds
            .Select(planId => TalkService.Create(
                planId,
                request.ShipmentVolumeRange,
                request.FirstName,
                request.LastName,
                request.CountryId,
                request.GovernmentId,
                request.CityId ?? Guid.Empty,
                request.VillageId,
                request.PostalCode,
                request.PhoneNumber,
                request.WorkEmail,
                request.JobTitle,
                request.CompanyName,
                request.CompanyAddress,
                request.WebsiteUrl,
                request.AdditionalInformation))
            .ToList();

        db.TalkServices.AddRange(entities);
        await db.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(
            entities.First().Id,
            localizer["TalkServiceCreatedSuccessfully"]);
    }
}
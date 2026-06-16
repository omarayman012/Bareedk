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
        var planIds = request.ServiceBusinessPlanIds
            .Distinct()
            .ToList();

        var alreadyExists = await db.TalkServices
            .AnyAsync(x =>
                x.WorkEmail == request.WorkEmail &&
                planIds.Contains(x.ServiceBusinessPlanId),
                cancellationToken);

        if (alreadyExists)
            return Result<Guid>.Failure(localizer["TalkServiceAlreadyExists"]);

        var existingPlanIds = await db.ServiceBusinessPlans
            .Where(x => planIds.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        if (existingPlanIds.Count != planIds.Count)
            return Result<Guid>.Failure(localizer["ServiceBusinessPlanNotFound"]);

        var countryExists = await db.Countries
            .AnyAsync(x => x.CountryId == request.CountryId, cancellationToken);

        if (!countryExists)
            return Result<Guid>.Failure(localizer["CountryNotFound"]);

        var governmentExists = await db.Governments
            .AnyAsync(x =>
                x.GovernmentId == request.GovernmentId &&
                x.CountryId == request.CountryId,
                cancellationToken);

        if (!governmentExists)
            return Result<Guid>.Failure(localizer["GovernmentNotFound"]);

        if (request.CityId.HasValue)
        {
            var cityExists = await db.Cities
                .AnyAsync(x =>
                    x.CityId == request.CityId.Value &&
                    x.GovernmentId == request.GovernmentId,
                    cancellationToken);

            if (!cityExists)
                return Result<Guid>.Failure(localizer["CityNotFound"]);
        }

        if (request.VillageId.HasValue)
        {
            var villageExists = await db.Villages
                .AnyAsync(x =>
                    x.VillageId == request.VillageId.Value &&
                    (!request.CityId.HasValue || x.CityId == request.CityId.Value),
                    cancellationToken);

            if (!villageExists)
                return Result<Guid>.Failure(localizer["VillageNotFound"]);
        }

        var entities = planIds
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
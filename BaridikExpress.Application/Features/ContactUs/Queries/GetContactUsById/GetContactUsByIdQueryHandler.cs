using BaridikExpress.Application.Features.ContactUs.DTOs;
using Microsoft.EntityFrameworkCore;

namespace BaridikExpress.Application.Features.ContactUs.Queries.GetContactUsById;

public sealed class GetContactUsByIdQueryHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<GetContactUsByIdQuery, Result<ContactUsResponse>>
{
    public async Task<Result<ContactUsResponse>> Handle(
        GetContactUsByIdQuery request,
        CancellationToken cancellationToken)
    {
        var response = await db.ContactUs
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new ContactUsResponse(
                x.Id,
                x.Name,
                x.Email,
                x.Phone,
                x.Message,
                x.IsRead,
                x.CreatedAt))
            .FirstOrDefaultAsync(cancellationToken);

        if (response is null)
            return Result<ContactUsResponse>.Failure(localizer["ContactUsNotFound"], 404);

        return Result<ContactUsResponse>
            .Success(response, localizer["ContactUsRetrievedSuccessfully"]);
    }
}
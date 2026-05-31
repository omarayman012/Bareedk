using BaridikExpress.Domain.Entities.ContactUs;

namespace BaridikExpress.Application.Features.ContactUs.Commands.SendContactUs;

public sealed class SendContactUsCommandHandler(
    IApplicationDbContext db,
    IStringLocalizer localizer)
    : IRequestHandler<SendContactUsCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(
        SendContactUsCommand request,
        CancellationToken cancellationToken)
    {
        var contactUs = Domain.Entities.ContactUs.ContactUs.Create(
            request.Name,
            request.Email,
            request.Phone,
            request.Message);

        await db.ContactUs.AddAsync(contactUs, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true, localizer["MessageSentSuccessfully"], 201);
    }
}
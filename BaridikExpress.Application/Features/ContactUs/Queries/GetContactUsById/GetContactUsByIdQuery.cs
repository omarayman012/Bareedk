using BaridikExpress.Application.Features.ContactUs.DTOs;

namespace BaridikExpress.Application.Features.ContactUs.Queries.GetContactUsById;

public sealed record GetContactUsByIdQuery(
    Guid Id
) : IRequest<Result<ContactUsResponse>>;
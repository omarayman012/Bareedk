using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetFAQById;

public sealed record GetFAQByIdQuery(Guid Id)
    : IRequest<Result<FAQResponse>>;
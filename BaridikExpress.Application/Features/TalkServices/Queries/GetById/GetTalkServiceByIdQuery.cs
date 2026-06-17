using BaridikExpress.Application.Common.Models;
using BaridikExpress.Application.Features.TalkServices.DTOs;
using MediatR;

namespace BaridikExpress.Application.Features.TalkServices.Queries.GetById;

public sealed record GetTalkServiceByIdQuery(Guid Id)
    : IRequest<Result<GetTalkServiceDto>>;
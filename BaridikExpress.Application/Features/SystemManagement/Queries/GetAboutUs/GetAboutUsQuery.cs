using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetAboutUs;

public sealed record GetAboutUsQuery : IRequest<Result<AboutUsResponse>>;
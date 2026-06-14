using BaridikExpress.Application.Features.SystemManagement.DTOs;

namespace BaridikExpress.Application.Features.SystemManagement.Queries.GetGeneralCompanySettings;

public sealed record GetGeneralCompanySettingsQuery
    : IRequest<Result<GeneralCompanySettingsResponse>>;
namespace BaridikExpress.Application.Features.SystemManagement.Commands.UpdateGeneralCompanySettings;

public sealed record UpdateGeneralCompanySettingsCommand(
    int? WorkingHoursDuration,
    int? NumberOfRejectedShipmentsByDelivery
) : IRequest<Result<bool>>;
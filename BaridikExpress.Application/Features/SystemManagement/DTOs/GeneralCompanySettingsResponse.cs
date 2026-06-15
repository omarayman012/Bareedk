namespace BaridikExpress.Application.Features.SystemManagement.DTOs;

public sealed record GeneralCompanySettingsResponse(
    Guid Id,
    int WorkingHoursDuration,
    int NumberOfRejectedShipmentsByDelivery,
    string? UpdatedBy,
    DateTime? UpdatedAt
);
using BaridikExpress.Domain.Entities.Base;

namespace BaridikExpress.Domain.Entities.SystemManagment;

public class GeneralCompanySettings : BaseEntity
{
    public Guid Id { get; private set; }
    public int WorkingHoursDuration { get; private set; }
    public int NumberOfRejectedShipmentsByDelivery { get; private set; }

    private GeneralCompanySettings() { }

    public static GeneralCompanySettings Create(
        int workingHoursDuration,
        int numberOfRejectedShipmentsByDelivery)
    {
        if (workingHoursDuration <= 0)
            throw new ArgumentOutOfRangeException(nameof(workingHoursDuration),
                "Working hours must be greater than zero.");

        if (numberOfRejectedShipmentsByDelivery < 0)
            throw new ArgumentOutOfRangeException(nameof(numberOfRejectedShipmentsByDelivery),
                "Number of rejected shipments cannot be negative.");

        return new GeneralCompanySettings
        {
            Id = Guid.NewGuid(),
            WorkingHoursDuration = workingHoursDuration,
            NumberOfRejectedShipmentsByDelivery = numberOfRejectedShipmentsByDelivery,
        };
    }

    public void Update(
        int? workingHoursDuration = null,
        int? numberOfRejectedShipmentsByDelivery = null)
    {
        if (workingHoursDuration.HasValue)
        {
            if (workingHoursDuration.Value <= 0)
                throw new ArgumentOutOfRangeException(nameof(workingHoursDuration),
                    "Working hours must be greater than zero.");

            WorkingHoursDuration = workingHoursDuration.Value;
        }

        if (numberOfRejectedShipmentsByDelivery.HasValue)
        {
            if (numberOfRejectedShipmentsByDelivery.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfRejectedShipmentsByDelivery),
                    "Number of rejected shipments cannot be negative.");

            NumberOfRejectedShipmentsByDelivery = numberOfRejectedShipmentsByDelivery.Value;
        }
    }
}
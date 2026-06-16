using System.Text.Json.Serialization;

namespace BaridikExpress.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]

public enum StatisticsFilter
{
    Overall,
    Today,
    ThisWeek,
    ThisMonth,
    Last3Months,
    Last6Months,
    Last9Months,
    ThisYear
}
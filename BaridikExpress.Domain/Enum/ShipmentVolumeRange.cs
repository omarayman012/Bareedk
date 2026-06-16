using System.ComponentModel;

namespace BaridikExpress.Domain.Enum
{
    public enum ShipmentVolumeRange
    {
        [Description("Not Found")]
        NotFound = 0,

        [Description("0 - 20")]
        Range_0_20 = 1,

        [Description("21 - 50")]
        Range_21_50 = 2,

        [Description("51 - 100")]
        Range_51_100 = 3,

        [Description("101 - 500")]
        Range_101_500 = 4,

        [Description("501 - 1000")]
        Range_501_1000 = 5,

        [Description("1001 - 100,000")]
        Range_1001_100000 = 6,

        [Description("+100000")]
        Plus_100000 = 7
    }
}
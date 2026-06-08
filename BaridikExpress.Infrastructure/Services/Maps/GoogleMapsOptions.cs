using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Infrastructure.Services.Maps
{
    public sealed class GoogleMapsOptions
    {
        public const string SectionName = "GoogleMaps";
        public string ApiKey { get; set; } = string.Empty;
    }
}

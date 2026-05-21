using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaridikExpress.Application.DTOs;

namespace BaridikExpress.Application.Common.Helpers
{
    public class LocalizedEntityDto
    {
        public Guid Id { get; set; }
        public LocalizeLang Name { get; set; } = default!;
    }
}

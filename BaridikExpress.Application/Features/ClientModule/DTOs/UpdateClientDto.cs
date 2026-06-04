using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ClientModule.DTOs
{
    public class UpdateClientDto
    {
        public string UserId { get; set; }  

        public string FullName { get; set; }

        public Guid CareerFieldId { get; set; }

        public string CompanyName { get; set; }
        public string CompanyLink { get; set; }

       
    }
}

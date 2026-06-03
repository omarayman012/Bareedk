using BaridikExpress.Application.Features.ClientModule.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ClientModule.Commond
{
    public class UpdateClientCommand : IRequest<Result<GetClientByIdDto>>
    {
        public UpdateClientDto Dto { get; set; }
    }
}

using BaridikExpress.Application.Features.ClientModule.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.ClientModule.Queries
{
    public class GetClientByIdQuery : IRequest<Result<GetClientByIdDto>>
    {
        public string Id { get; set; }
    }
}

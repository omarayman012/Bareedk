using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.TalkServices.Commands.Delete
{
    public sealed record DeleteTalkServicesCommand(List<Guid> Ids) : IRequest<Result<bool>>;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Command
{
    public class DeletePublishingHouseCommand : IRequest<Result<bool>>
    {
        public List<Guid> Ids { get; set; } = new();
    }
}


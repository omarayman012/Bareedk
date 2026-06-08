using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.PublishingHouseModule.Command
{
    public class ChangePublishingHouseStatusCommand : IRequest<Result<string>>
    {
        public Guid Id { get; set; }
    }
}
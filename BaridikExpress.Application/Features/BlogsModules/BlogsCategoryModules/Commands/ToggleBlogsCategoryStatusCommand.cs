using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BaridikExpress.Application.Features.BlogsModules.BlogsCategoryModules.Commands
{
    public class ToggleBlogsCategoryStatusCommand : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }
}

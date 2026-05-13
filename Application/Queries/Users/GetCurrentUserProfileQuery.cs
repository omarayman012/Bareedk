using BaridikExpress.Application.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Queries.Users
{
    public record GetCurrentUserProfileQuery() : IRequest<Result<CurrentUserResponseDTO>>;
}

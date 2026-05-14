using BaridikExpress.Application.Features.Auth.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Users.Queries.GetCurrentUserProfile
{
    public record GetCurrentUserProfileQuery() : IRequest<Result<CurrentUserResponseDTO>>;
}

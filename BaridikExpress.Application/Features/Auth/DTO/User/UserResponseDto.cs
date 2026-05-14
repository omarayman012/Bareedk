using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Features.Auth.DTO.User
{
       public record UserResponseDto(
        string Id,
        string FullName,
        string Email,
        bool IsVerified
);

    }

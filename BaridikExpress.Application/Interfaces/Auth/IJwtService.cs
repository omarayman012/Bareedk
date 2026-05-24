using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.Auth
{
    public interface IJwtService
    {
        Task<string> GenerateToken(User user, string role);
    }
}

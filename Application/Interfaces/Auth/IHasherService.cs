using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaridikExpress.Application.Interfaces.Auth
{
    public interface IHasherService
    {
        string Hash(string value);
        bool Verify(string hashedValue, string providedValue);
    }
}

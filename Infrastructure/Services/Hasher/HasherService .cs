using BaridikExpress.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Identity;

namespace BaridikExpress.Infrastructure.Services.Hasher
{
    public class HasherService : IHasherService
    {
        private readonly PasswordHasher<string> _hasher = new();

        public string Hash(string value)
        {
            return _hasher.HashPassword(null!, value);
        }

        public bool Verify(string hashedValue, string providedValue)
        {
            var result = _hasher.VerifyHashedPassword(null!, hashedValue, providedValue);
            return result == PasswordVerificationResult.Success;
        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace AuthApi.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}

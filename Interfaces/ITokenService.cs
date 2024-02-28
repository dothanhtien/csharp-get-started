using CSharpGetStarted.Entities;

namespace CSharpGetStarted.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}

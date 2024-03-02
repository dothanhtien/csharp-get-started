using CSharpGetStarted.Entities;

namespace CSharpGetStarted.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}

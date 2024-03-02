using Microsoft.AspNetCore.Identity;

namespace CSharpGetStarted.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}

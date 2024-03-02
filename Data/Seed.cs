using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using CSharpGetStarted.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSharpGetStarted.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

            var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            foreach (var user in users)
            {
                user.UserName = user.UserName.Trim().ToLower();

                context.Users.Add(user);
            }

            await context.SaveChangesAsync();
        }
    }
}

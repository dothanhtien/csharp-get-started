using CSharpGetStarted.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSharpGetStarted.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}

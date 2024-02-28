using CSharpGetStarted.Data;
using CSharpGetStarted.Interfaces;
using CSharpGetStarted.Services;
using Microsoft.EntityFrameworkCore;

namespace CSharpGetStarted.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}

using CSharpGetStarted.Data;
using CSharpGetStarted.Helpers;
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<LogUserActivity>();
            services.AddScoped<ILikeRepository, LikeRepository>();

            return services;
        }
    }
}

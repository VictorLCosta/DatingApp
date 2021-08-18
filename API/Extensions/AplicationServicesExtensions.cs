using API.Data;
using API.Interfaces;
using API.Interfaces.Contracts;
using API.Repositories;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using API.Helpers;

namespace API.Extensions
{
    public static class AplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPhotoService, PhotoService>();

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion

            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            services.AddDbContext<DataContext>(opt => 
            {
                opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}
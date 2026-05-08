using BE.Application.Interfaces;
using BE.Application.Services;
using BE.Domain.Interfaces;
using BE.Infrastructure.Authentication;
using BE.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BE.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Application Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminService, AdminService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();

            // Authentication
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}

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
            services.AddScoped<IWorkflowExecutionService, WorkflowExecutionService>();

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<IWorkflowRepository, WorkflowRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IApprovalRepository, ApprovalRepository>();
            
            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Authentication
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}

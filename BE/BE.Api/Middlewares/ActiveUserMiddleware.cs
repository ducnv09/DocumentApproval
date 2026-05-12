using BE.Domain.Interfaces;
using System.Security.Claims;

namespace BE.Api.Middlewares
{
    public class ActiveUserMiddleware
    {
        private readonly RequestDelegate _next;

        public ActiveUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid userId))
                {
                    // Resolve Repository from RequestServices (Scoped)
                    var userRepository = context.RequestServices.GetRequiredService<IUserRepository>();
                    var user = await userRepository.GetByIdAsync(userId, context.RequestAborted);

                    if (user == null || !user.IsActive)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsJsonAsync(new { message = "Tài khoản của bạn không khả dụng hoặc đã bị khóa." });
                        return; 
                    }
                }
            }

            await _next(context);
        }
    }

    public static class ActiveUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseActiveUserCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ActiveUserMiddleware>();
        }
    }
}

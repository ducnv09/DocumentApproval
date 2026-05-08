using System.Net;
using System.Text.Json;
using BE.Application.DTOs;
using BE.Domain.Exceptions;
using BE.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BE.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        // Khi một lỗi không được bắt (unhandled exception) xảy ra, pipeline sẽ tự động gọi vào hàm TryHandleAsync
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var traceId = httpContext.TraceIdentifier;

            // Phân loại lỗi
            var (statusCode, message, validationErrors) = exception switch
            {
                ValidationException valEx => (StatusCodes.Status400BadRequest, valEx.Message, valEx.Errors),
                UnauthorizedException authEx => (StatusCodes.Status401Unauthorized, authEx.Message, null),
                ForbiddenException forbidEx => (StatusCodes.Status403Forbidden, forbidEx.Message, null),
                NotFoundException notFoundEx => (StatusCodes.Status404NotFound, notFoundEx.Message, null),
                DomainException domainEx => (StatusCodes.Status422UnprocessableEntity, domainEx.Message, null),
                _ => (StatusCodes.Status500InternalServerError, "Đã xảy ra lỗi hệ thống. Vui lòng liên hệ quản trị viên.", null)
            };

            if (statusCode >= 500)
            {
                _logger.LogError(exception, "[TraceId: {TraceId}] System Error: {Message}", traceId, exception.Message);
            }
            else
            {
                _logger.LogWarning("[TraceId: {TraceId}] Business Error: {StatusCode} - {Message}", traceId, statusCode, message);
            }

            // Standardized Response
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType = "application/json";

            // Đóng gói tất cả vào một DTO chung là Result.Failure
            var response = Result.Failure(message, traceId, validationErrors);

            // Sử dụng JsonSerializer.SerializeAsync trực tiếp để đảm bảo 100% dùng Source Generator
            // Cách này tường minh và tránh được sự nhập nhằng của các phương thức mở rộng
            await JsonSerializer.SerializeAsync(
                httpContext.Response.Body, 
                response, 
                SharedJsonContext.Default.ApiResponse, 
                cancellationToken);

            // Việc return true; ở cuối hàm mang ý nghĩa: "Tôi (GlobalExceptionHandler) đã xử lý xong cái exception này rồi, framework không cần phải lo về nó nữa, 
            // đừng văng lỗi màn hình vàng hay sập ứng dụng nhé". Nếu bạn return false, exception sẽ tiếp tục bị quăng ra ngoài.
            return true;
        }
    }
}

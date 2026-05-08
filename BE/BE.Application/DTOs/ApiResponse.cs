using System.Text.Json.Serialization;

namespace BE.Application.DTOs
{
    // Sử dụng record cho tính bất biến
    public record ApiResponse
    {
        public bool IsSuccess { get; init; }
        public string? Message { get; init; }
        public string? TraceId { get; init; }
        public IDictionary<string, string[]>? Errors { get; init; }
    }

    public record ApiResponse<T> : ApiResponse
    {
        public T? Data { get; init; }
    }

    // Factory Pattern để khởi tạo Response mà không cần dùng từ khóa 'new' ở class kế thừa
    public static class Result
    {
        public static ApiResponse Success(string? message = null) 
            => new ApiResponse { IsSuccess = true, Message = message };

        public static ApiResponse<T> Success<T>(T data, string? message = null) 
            => new ApiResponse<T> { IsSuccess = true, Data = data, Message = message };

        public static ApiResponse Failure(string message, string? traceId = null, IDictionary<string, string[]>? errors = null) 
            => new ApiResponse { IsSuccess = false, Message = message, TraceId = traceId, Errors = errors };

        public static ApiResponse<T> Failure<T>(string message, string? traceId = null, IDictionary<string, string[]>? errors = null) 
            => new ApiResponse<T> { IsSuccess = false, Message = message, TraceId = traceId, Errors = errors };
    }

}
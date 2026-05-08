using System.Text.Json.Serialization;
using BE.Application.DTOs.Auth;
using BE.Application.DTOs.Admin;

// tối ưu hóa hiệu năng cực đại cho việc đọc/ghi JSON và hỗ trợ biên dịch Native AOT.
namespace BE.Application.DTOs
{
    // 1. Shared JSON Context: Chứa các DTO dùng chung của toàn hệ thống
    [JsonSerializable(typeof(ApiResponse))]
    [JsonSerializable(typeof(ApiResponse<object>))]
    public partial class SharedJsonContext : JsonSerializerContext { }
}

namespace BE.Application.DTOs.Auth
{
    // 2. Auth JSON Context: Chứa các DTO riêng của module Auth
    [JsonSerializable(typeof(ApiResponse<LoginResponse>))]
    [JsonSerializable(typeof(LoginRequest))]
    [JsonSerializable(typeof(LoginResponse))]
    public partial class AuthJsonContext : JsonSerializerContext { }
}

namespace BE.Application.DTOs.Admin
{
    // 3. Admin JSON Context: Chứa các DTO riêng của module Admin
    [JsonSerializable(typeof(ApiResponse<UserDto>))]
    [JsonSerializable(typeof(ApiResponse<List<UserDto>>))]
    [JsonSerializable(typeof(ApiResponse<List<GroupDto>>))]
    [JsonSerializable(typeof(ApiResponse<List<PositionDto>>))]
    [JsonSerializable(typeof(UserDto))]
    [JsonSerializable(typeof(CreateUserDto))]
    [JsonSerializable(typeof(UpdateUserDto))]
    [JsonSerializable(typeof(GroupDto))]
    [JsonSerializable(typeof(PositionDto))]
    public partial class AdminJsonContext : JsonSerializerContext { }
}

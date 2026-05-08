using BE.Application.DTOs.Auth;
using BE.Domain.Entities;

namespace BE.Application.Mapping
{
    public static class AuthMapper
    {
        public static LoginResponse ToLoginResponse(this User user, string token)
        {
            return new LoginResponse
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Token = token
            };
        }
    }
}

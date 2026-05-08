using BE.Application.DTOs.Auth;
using BE.Application.Interfaces;
using BE.Domain.Interfaces;
using BE.Domain.Exceptions;
using BE.Application.Mapping;
using BC = BCrypt.Net.BCrypt;

namespace BE.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;

        public AuthService(IUserRepository userRepository, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            // Use Unauthorized (401) for credential mismatch
            if (user == null || !BC.Verify(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedException();
            }

            // Use Forbidden (403) for locked accounts
            if (!user.IsActive)
            {
                throw new ForbiddenException("Tài khoản của bạn đã bị khóa.");
            }

            var token = _jwtProvider.GenerateToken(user);

            return user.ToLoginResponse(token);
        }
    }
}

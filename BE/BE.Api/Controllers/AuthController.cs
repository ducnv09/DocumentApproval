using BE.Application.DTOs.Auth;
using BE.Application.Interfaces;
using BE.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace BE.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            //  Controller trả về object ➡️ ASP.NET Core tự vào TypeInfoResolverChain 
            //  để tìm bản thiết kế khớp với object đó ➡️ Chạy code đã sinh sẵn ➡️ Trả JSON về cho Client.
            return Ok(Result.Success(response, "Đăng nhập thành công."));
        }
    }
}

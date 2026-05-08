using BE.Application.DTOs.Admin;
using BE.Application.Interfaces;
using BE.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BE.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers() 
            => Ok(Result.Success(await _adminService.GetUsersAsync()));

        [HttpPost("users")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto) 
            => Ok(Result.Success(await _adminService.CreateUserAsync(dto), "Tạo người dùng thành công."));

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto dto)
        {
            await _adminService.UpdateUserAsync(id, dto);
            return Ok(Result.Success("Cập nhật thành công."));
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _adminService.DeleteUserAsync(id);
            return Ok(Result.Success("Xóa thành công."));
        }

        [HttpGet("groups")]
        public async Task<IActionResult> GetGroups() 
            => Ok(Result.Success(await _adminService.GetGroupsAsync()));

        [HttpPost("groups")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupDto dto) 
            => Ok(Result.Success(await _adminService.CreateGroupAsync(dto), "Tạo nhóm thành công."));

        [HttpGet("positions")]
        public async Task<IActionResult> GetPositions() 
            => Ok(Result.Success(await _adminService.GetPositionsAsync()));

        [HttpPost("positions")]
        public async Task<IActionResult> CreatePosition([FromBody] CreatePositionDto dto) 
            => Ok(Result.Success(await _adminService.CreatePositionAsync(dto), "Tạo chức danh thành công."));

        [HttpPost("assign-group")]
        public async Task<IActionResult> AssignUserToGroup([FromBody] AssignUserGroupDto dto)
        {
            await _adminService.AssignUserToGroupAsync(dto);
            return Ok(Result.Success("Phân quyền nhóm thành công."));
        }
    }
}

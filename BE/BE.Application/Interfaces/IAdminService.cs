using BE.Application.DTOs.Admin;

namespace BE.Application.Interfaces
{
    public interface IAdminService
    {
        // User Management
        Task<List<UserDto>> GetUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto dto);
        Task UpdateUserAsync(Guid id, UpdateUserDto dto);
        Task DeleteUserAsync(Guid id);

        // Group Management
        Task<List<GroupDto>> GetGroupsAsync();
        Task<GroupDto> CreateGroupAsync(CreateGroupDto dto);

        // Position Management
        Task<List<PositionDto>> GetPositionsAsync();
        Task<PositionDto> CreatePositionAsync(CreatePositionDto dto);

        // UserGroup Assignment
        Task AssignUserToGroupAsync(AssignUserGroupDto dto);
    }
}

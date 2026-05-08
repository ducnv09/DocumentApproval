using BE.Application.DTOs.Admin;
using BE.Domain.Entities;

namespace BE.Application.Mapping
{
    public static class AdminMapper
    {
        // User Mappings
        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                IsActive = user.IsActive
            };
        }

        public static List<UserDto> ToDtoList(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToDto()).ToList();
        }

        // Group Mappings
        public static GroupDto ToDto(this Group group)
        {
            return new GroupDto
            {
                Id = group.Id,
                Name = group.Name,
                Code = group.Code
            };
        }

        public static List<GroupDto> ToDtoList(this IEnumerable<Group> groups)
        {
            return groups.Select(g => g.ToDto()).ToList();
        }

        // Position Mappings
        public static PositionDto ToDto(this Position position)
        {
            return new PositionDto
            {
                Id = position.Id,
                Name = position.Name
            };
        }

        public static List<PositionDto> ToDtoList(this IEnumerable<Position> positions)
        {
            return positions.Select(p => p.ToDto()).ToList();
        }
    }
}

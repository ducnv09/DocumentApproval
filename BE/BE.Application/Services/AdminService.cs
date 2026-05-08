using BE.Application.DTOs.Admin;
using BE.Application.Interfaces;
using BE.Domain.Interfaces;
using BE.Domain.Entities;
using BE.Domain.Exceptions;
using BE.Application.Mapping;
using BC = BCrypt.Net.BCrypt;

namespace BE.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IUserGroupRepository _userGroupRepository;

        public AdminService(
            IUserRepository userRepository, 
            IGroupRepository groupRepository, 
            IPositionRepository positionRepository,
            IUserGroupRepository userGroupRepository)
        {
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _positionRepository = positionRepository;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.ToDtoList();
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
        {
            if (await _userRepository.ExistsAsync(dto.Username))
                throw new DomainException("Tên đăng nhập đã tồn tại.");

            var user = new User(dto.Username, dto.FullName, dto.Email, dto.IsAdmin);
            user.SetPassword(BC.HashPassword(dto.Password));

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user.ToDto();
        }

        public async Task UpdateUserAsync(Guid id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("Người dùng", id);

            user.UpdateInfo(dto.FullName, dto.Email);
            if (dto.IsActive) 
            {
                user.Activate();
            } 
            else 
            {
                user.Deactivate();
            }

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("Người dùng", id);

            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<List<GroupDto>> GetGroupsAsync()
        {
            var groups = await _groupRepository.GetAllAsync();
            return groups.ToDtoList();
        }

        public async Task<GroupDto> CreateGroupAsync(CreateGroupDto dto)
        {
            var group = new Group(dto.Name, dto.Code);
            await _groupRepository.AddAsync(group);
            await _groupRepository.SaveChangesAsync();

            return group.ToDto();
        }

        public async Task<List<PositionDto>> GetPositionsAsync()
        {
            var positions = await _positionRepository.GetAllAsync();
            return positions.ToDtoList();
        }

        public async Task<PositionDto> CreatePositionAsync(CreatePositionDto dto)
        {
            var position = new Position(dto.Name);
            await _positionRepository.AddAsync(position);
            await _positionRepository.SaveChangesAsync();

            return position.ToDto();
        }

        public async Task AssignUserToGroupAsync(AssignUserGroupDto dto)
        {
            var existing = await _userGroupRepository.GetAsync(dto.UserId, dto.GroupId);

            if (existing != null)
            {
                existing.UpdatePosition(dto.PositionId);
            }
            else
            {
                var userGroup = new UserGroup(dto.UserId, dto.GroupId, dto.PositionId);
                await _userGroupRepository.AddAsync(userGroup);
            }

            await _userGroupRepository.SaveChangesAsync();
        }
    }
}

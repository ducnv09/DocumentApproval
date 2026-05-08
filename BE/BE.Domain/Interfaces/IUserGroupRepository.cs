using BE.Domain.Entities;

namespace BE.Domain.Interfaces
{
    public interface IUserGroupRepository
    {
        Task<UserGroup?> GetAsync(Guid userId, Guid groupId);
        Task AddAsync(UserGroup userGroup);
        Task SaveChangesAsync();
    }
}

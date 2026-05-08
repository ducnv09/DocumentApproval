using BE.Domain.Entities;

namespace BE.Domain.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group?> GetByIdAsync(Guid id);
        Task<List<Group>> GetAllAsync();
        Task AddAsync(Group group);
        Task SaveChangesAsync();
    }
}

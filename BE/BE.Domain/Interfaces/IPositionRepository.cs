using BE.Domain.Entities;

namespace BE.Domain.Interfaces
{
    public interface IPositionRepository
    {
        Task<Position?> GetByIdAsync(Guid id);
        Task<List<Position>> GetAllAsync();
        Task AddAsync(Position position);
        Task SaveChangesAsync();
    }
}

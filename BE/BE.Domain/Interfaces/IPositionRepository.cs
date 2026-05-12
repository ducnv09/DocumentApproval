using BE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Domain.Interfaces
{
    public interface IPositionRepository
    {
        Task<Position?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Position>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Position position, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

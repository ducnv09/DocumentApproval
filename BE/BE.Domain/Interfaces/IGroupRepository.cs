using BE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Domain.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Group group, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

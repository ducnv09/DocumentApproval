using BE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Domain.Interfaces
{
    public interface IUserGroupRepository
    {
        Task<UserGroup?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default);
        Task<List<UserGroup>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task AddAsync(UserGroup userGroup, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

using BE.Domain.Entities;
using BE.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Domain.Interfaces
{
    public interface IApprovalRepository
    {
        Task<List<Approval>> GetPendingByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default);
        Task<List<Approval>> GetApprovedByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default);
        Task AddAsync(Approval approval, CancellationToken cancellationToken = default);
        Task UpdateAsync(Approval approval, CancellationToken cancellationToken = default);
    }
}

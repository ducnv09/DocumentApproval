using BE.Domain.Entities;
using BE.Domain.Enums;
using BE.Domain.Interfaces;
using BE.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Infrastructure.Persistence.Repositories
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ApplicationDbContext _context;

        public ApprovalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Approval>> GetPendingByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            return await _context.Approvals
                .Where(a => a.DocumentId == documentId && a.ActionType == ActionType.Pending)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Approval>> GetApprovedByDocumentIdAsync(Guid documentId, CancellationToken cancellationToken = default)
        {
            return await _context.Approvals
                .Where(a => a.DocumentId == documentId && a.ActionType == ActionType.Approved)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Approval approval, CancellationToken cancellationToken = default)
        {
            await _context.Approvals.AddAsync(approval, cancellationToken);
        }

        public Task UpdateAsync(Approval approval, CancellationToken cancellationToken = default)
        {
            _context.Approvals.Update(approval);
            return Task.CompletedTask;
        }
    }
}

using BE.Domain.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Application.Interfaces
{
    public interface IWorkflowExecutionService
    {
        Task StartWorkflowAsync(Guid documentId, Guid workflowId, CancellationToken cancellationToken = default);
        Task ProcessApprovalAsync(Guid documentId, Guid approverId, ActionType action, string? reason, byte[]? signature, CancellationToken cancellationToken = default);
    }
}

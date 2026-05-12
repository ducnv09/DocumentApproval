using System.Threading;
using System.Threading.Tasks;
using BE.Domain.Interfaces;

namespace BE.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IGroupRepository Groups { get; }
        IPositionRepository Positions { get; }
        IUserGroupRepository UserGroups { get; }
        IWorkflowRepository Workflows { get; }
        IDocumentRepository Documents { get; }
        IApprovalRepository Approvals { get; }

        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
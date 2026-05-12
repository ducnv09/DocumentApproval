using System.Data;
using System.Threading;
using System.Threading.Tasks;
using BE.Domain.Interfaces;
using BE.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BE.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository users,
            IGroupRepository groups,
            IPositionRepository positions,
            IUserGroupRepository userGroups,
            IWorkflowRepository workflows,
            IDocumentRepository documents,
            IApprovalRepository approvals)
        {
            _context = context;
            Users = users;
            Groups = groups;
            Positions = positions;
            UserGroups = userGroups;
            Workflows = workflows;
            Documents = documents;
            Approvals = approvals;
        }

        public IUserRepository Users { get; }
        public IGroupRepository Groups { get; }
        public IPositionRepository Positions { get; }
        public IUserGroupRepository UserGroups { get; }
        public IWorkflowRepository Workflows { get; }
        public IDocumentRepository Documents { get; }
        public IApprovalRepository Approvals { get; }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

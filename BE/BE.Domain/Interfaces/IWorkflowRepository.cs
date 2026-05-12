using BE.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Domain.Interfaces
{
    public interface IWorkflowRepository
    {
        Task<Workflow?> GetByIdWithStepsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Step?> GetStepByIdAsync(Guid stepId, CancellationToken cancellationToken = default);
    }
}

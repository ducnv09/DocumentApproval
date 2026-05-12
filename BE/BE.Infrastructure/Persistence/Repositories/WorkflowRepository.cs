using BE.Domain.Entities;
using BE.Domain.Interfaces;
using BE.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Infrastructure.Persistence.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        private readonly ApplicationDbContext _context;

        public WorkflowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Workflow?> GetByIdWithStepsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Workflows.Include(w => w.Steps).FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }

        public async Task<Step?> GetStepByIdAsync(Guid stepId, CancellationToken cancellationToken = default)
        {
            return await _context.Steps.FirstOrDefaultAsync(s => s.Id == stepId, cancellationToken);
        }
    }
}

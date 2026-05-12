using BE.Domain.Interfaces;
using BE.Domain.Entities;
using BE.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BE.Infrastructure.Persistence.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ApplicationDbContext _context;
        public GroupRepository(ApplicationDbContext context) => _context = context;

        public async Task<Group?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await _context.Groups.FindAsync(new object[] { id }, cancellationToken);
        public async Task<List<Group>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Groups.ToListAsync(cancellationToken);
        public async Task AddAsync(Group group, CancellationToken cancellationToken = default) => await _context.Groups.AddAsync(group, cancellationToken);
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    }
}

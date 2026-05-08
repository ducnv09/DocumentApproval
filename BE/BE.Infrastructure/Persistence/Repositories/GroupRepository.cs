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

        public async Task<Group?> GetByIdAsync(Guid id) => await _context.Groups.FindAsync(id);
        public async Task<List<Group>> GetAllAsync() => await _context.Groups.ToListAsync();
        public async Task AddAsync(Group group) => await _context.Groups.AddAsync(group);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

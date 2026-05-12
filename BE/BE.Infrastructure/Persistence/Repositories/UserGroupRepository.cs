using BE.Domain.Interfaces;
using BE.Domain.Entities;
using BE.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BE.Infrastructure.Persistence.Repositories
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly ApplicationDbContext _context;
        public UserGroupRepository(ApplicationDbContext context) => _context = context;

        public async Task<UserGroup?> GetAsync(Guid userId, Guid groupId, CancellationToken cancellationToken = default) => 
            await _context.UserGroups.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId, cancellationToken);
        
        public async Task<List<UserGroup>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
            await _context.UserGroups.Where(ug => ug.UserId == userId).ToListAsync(cancellationToken);
        public async Task AddAsync(UserGroup userGroup, CancellationToken cancellationToken = default) => await _context.UserGroups.AddAsync(userGroup, cancellationToken);
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    }
}

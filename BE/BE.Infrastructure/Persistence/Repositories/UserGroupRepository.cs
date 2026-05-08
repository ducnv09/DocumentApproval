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

        public async Task<UserGroup?> GetAsync(Guid userId, Guid groupId) => 
            await _context.UserGroups.FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GroupId == groupId);
        public async Task AddAsync(UserGroup userGroup) => await _context.UserGroups.AddAsync(userGroup);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

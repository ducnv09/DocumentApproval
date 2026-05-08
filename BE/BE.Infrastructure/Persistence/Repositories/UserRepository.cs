using BE.Domain.Interfaces;
using BE.Domain.Entities;
using BE.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BE.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context) => _context = context;

        public async Task<User?> GetByIdAsync(Guid id) => await _context.Users.FindAsync(id);
        public async Task<User?> GetByUsernameAsync(string username) => await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        public async Task<List<User>> GetAllAsync() => await _context.Users.ToListAsync();
        public async Task AddAsync(User user) => await _context.Users.AddAsync(user);
        public async Task UpdateAsync(User user) => _context.Users.Update(user);
        public async Task DeleteAsync(User user) => _context.Users.Remove(user);
        public async Task<bool> ExistsAsync(string username) => await _context.Users.AnyAsync(u => u.Username == username);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

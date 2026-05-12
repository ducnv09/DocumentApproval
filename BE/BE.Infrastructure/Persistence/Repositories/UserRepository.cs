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

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await _context.Users.FindAsync(new object[] { id }, cancellationToken);
        public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default) => await _context.Users.FirstOrDefaultAsync(u => u.Username == username, cancellationToken);
        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Users.ToListAsync(cancellationToken);
        public async Task AddAsync(User user, CancellationToken cancellationToken = default) => await _context.Users.AddAsync(user, cancellationToken);
        public Task UpdateAsync(User user, CancellationToken cancellationToken = default) 
        {
            _context.Users.Update(user);
            return Task.CompletedTask;
        }
        public Task DeleteAsync(User user, CancellationToken cancellationToken = default) 
        {
            _context.Users.Remove(user);
            return Task.CompletedTask;
        }
        public async Task<bool> ExistsAsync(string username, CancellationToken cancellationToken = default) => await _context.Users.AnyAsync(u => u.Username == username, cancellationToken);
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    }
}

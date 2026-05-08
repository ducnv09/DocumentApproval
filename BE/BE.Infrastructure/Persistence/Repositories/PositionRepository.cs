using BE.Domain.Interfaces;
using BE.Domain.Entities;
using BE.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BE.Infrastructure.Persistence.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly ApplicationDbContext _context;
        public PositionRepository(ApplicationDbContext context) => _context = context;

        public async Task<Position?> GetByIdAsync(Guid id) => await _context.Positions.FindAsync(id);
        public async Task<List<Position>> GetAllAsync() => await _context.Positions.ToListAsync();
        public async Task AddAsync(Position position) => await _context.Positions.AddAsync(position);
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}

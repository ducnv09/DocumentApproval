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

        public async Task<Position?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await _context.Positions.FindAsync(new object[] { id }, cancellationToken);
        public async Task<List<Position>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Positions.ToListAsync(cancellationToken);
        public async Task AddAsync(Position position, CancellationToken cancellationToken = default) => await _context.Positions.AddAsync(position, cancellationToken);
        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    }
}

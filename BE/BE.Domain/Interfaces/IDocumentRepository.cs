using BE.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BE.Domain.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task UpdateAsync(Document document, CancellationToken cancellationToken = default);
    }
}

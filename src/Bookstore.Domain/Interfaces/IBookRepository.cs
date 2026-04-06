using Bookstore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Domain.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken ct = default);
        Task AddAsync(Book book, CancellationToken ct = default);
        Task UpdateAsync(Book book, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}

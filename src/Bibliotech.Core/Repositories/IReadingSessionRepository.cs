using System;
using Bibliotech.Core.Models;

namespace Bibliotech.Core.Repositories;

public interface IReadingSessionRepository
{
          Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
          Task<IEnumerable<ReadingSession>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
          Task<IEnumerable<ReadingSession>> GetByBookIdAsync(string bookId, CancellationToken cancellationToken = default);
          Task<IEnumerable<ReadingSession>> GetUserSessionsInDataRangeAsync(string userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
          Task AddAsync(ReadingSession session, CancellationToken cancellationToken = default);
          Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default);
          Task DeleteAsync(string id, CancellationToken cancellationToken = default);
}

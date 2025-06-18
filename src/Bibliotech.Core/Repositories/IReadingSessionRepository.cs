using System;
using Bibliotech.Core.Models;

namespace Bibliotech.Core.Repositories;

public interface IReadingSessionRepository
{
          Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToke = default);
          Task<IEnumerable<ReadingSession>> GetByUserIdAsync(string userId, CancellationToken cancellationToke = default);
          Task<IEnumerable<ReadingSession>> GetByBookIdAsync(string bookId, CancellationToken cancellationToke = default);
          Task<IEnumerable<ReadingSession>> GetUserSessionsInDataRangeAsync(string userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToke = default);
          Task AddAsync(ReadingSession session, CancellationToken cancellationToke = default);
          Task UpdateAsync(ReadingSession session, CancellationToken cancellationToke = default);
          Task DeleteAsync(string id, CancellationToken cancellationToke = default);
}

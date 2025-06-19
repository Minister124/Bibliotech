using System;
using Bibliotech.Core.Models;
using Bibliotech.Core.Repositories;
using Bibliotech.Modules.Infrastructure.Data;
using MongoDB.Driver;

namespace Bibliotech.Modules.Infrastructure.Repositories;

public class ReadingSessionRepository : IReadingSessionRepository
{
          private readonly MongoDBContext _context;

          public ReadingSessionRepository(MongoDBContext context)
          {
                    _context = context;
          }
          public async Task AddAsync(ReadingSession session, CancellationToken cancellationToken = default)
          {
                    await _context.ReadingSessions.InsertOneAsync(session, cancellationToken: cancellationToken);
          }

          public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
          {
                    await _context.ReadingSessions.DeleteOneAsync(s => s.Id == id, cancellationToken: cancellationToken);
          }

          public async Task<IEnumerable<ReadingSession>> GetByBookIdAsync(string bookId, CancellationToken cancellationToken = default)
          {
                    return await _context.ReadingSessions
                                        .Find(s => s.Id == bookId)
                                        .SortByDescending(s => s.StartTime)
                                        .ToListAsync(cancellationToken);
          }

          public async Task<ReadingSession?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
          {
                    return await _context.ReadingSessions.Find(s => s.Id == id)
                                                  .FirstOrDefaultAsync(cancellationToken);
          }

          public async Task<IEnumerable<ReadingSession>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
          {
                    return await _context.ReadingSessions.Find(s => s.UserId == userId)
                                                  .SortByDescending(s => s.StartTime)
                                                  .ToListAsync(cancellationToken);
          }

          public async Task<IEnumerable<ReadingSession>> GetUserSessionsInDataRangeAsync(string userId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
          {
                    return await _context.ReadingSessions
                                         .Find(s => s.UserId == userId && s.StartTime >= startDate && s.StartTime <= endDate)
                                         .SortByDescending(s => s.StartTime)
                                         .ToListAsync(cancellationToken);
          }

          public async Task UpdateAsync(ReadingSession session, CancellationToken cancellationToken = default)
          {
                    var filter = Builders<ReadingSession>.Filter.Eq(s => s.Id, session.Id);
                    await _context.ReadingSessions.ReplaceOneAsync(filter, session, cancellationToken: cancellationToken);
          }
}

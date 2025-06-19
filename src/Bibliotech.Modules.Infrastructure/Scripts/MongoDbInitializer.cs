using System;
using Bibliotech.Core.Models;
using Bibliotech.Modules.Infrastructure.Data;
using MongoDB.Driver;

namespace Bibliotech.Modules.Infrastructure.Scripts;

public static class MongoDbInitializer
{
          public static async Task InitializeAsync(MongoDBContext context)
          {
                 var readingSessionsCollection = context.ReadingSessions;

                    var readingSessionIndexes = new[]
                    {
                    new CreateIndexModel<ReadingSession>(
                              Builders<ReadingSession>.IndexKeys
                                                  .Ascending(x => x.UserId)
                                                  .Descending(x => x.StartTime), new CreateIndexOptions {Name = "idx_userId_startTime"}
                    ),
                    new CreateIndexModel<ReadingSession>(
                              Builders<ReadingSession>.IndexKeys
                                                  .Ascending(x => x.BookId),
                                                  new CreateIndexOptions {Name = "idx_bookId"}
                    ),
                    new CreateIndexModel<ReadingSession>(
                              Builders<ReadingSession>.IndexKeys
                                                  .Descending(x => x.StartTime),
                                                  new CreateIndexOptions {Name = "idx_startTime"}
                    )
                 };

                 await readingSessionsCollection.Indexes.CreateManyAsync(readingSessionIndexes);

                 //Creating Indexes for UserDNAProfiles collection
                 var userDNACollection = context.UserDNAProfiles;

                 var userDNAIndexes = new[]
                 {
                    new CreateIndexModel<UserDNAProfile>(
                              Builders<UserDNAProfile>.IndexKeys
                                                  .Ascending(x => x.UserId),
                                                  new CreateIndexOptions {Name = "idx_userId", Unique = true}
                    ),
                    new CreateIndexModel<UserDNAProfile>(
                              Builders<UserDNAProfile>.IndexKeys
                                                  .Descending(x => x.LastEvolved),
                                                  new CreateIndexOptions {Name = "idx_lastEvolved"}
                    )
                 };

                    await userDNACollection.Indexes.CreateManyAsync(userDNAIndexes);
          }
}

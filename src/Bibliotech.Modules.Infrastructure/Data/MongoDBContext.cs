using System;
using Bibliotech.Core.Models;
using MongoDB.Driver;

namespace Bibliotech.Modules.Infrastructure.Data;

public class MongoDBContext
{
          private readonly IMongoDatabase _database;

          public MongoDBContext(IMongoClient mongoClient, string databaseName)
          {
                    _database = mongoClient.GetDatabase(databaseName);
          }

          public IMongoCollection<ReadingSession> ReadingSessions => _database.GetCollection<ReadingSession>("readingSessions");
          public IMongoCollection<UserDNAProfile> UserDNAProfiles => _database.GetCollection<UserDNAProfile>("userDNAProfiles");
          public IMongoCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);
}

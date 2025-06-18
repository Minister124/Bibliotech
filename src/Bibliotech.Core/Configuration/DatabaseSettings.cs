using System;

namespace Bibliotech.Core.Configuration;

public class DatabaseSettings
{
          public const string SectionName = "DatabaseSettings";
          public string PostgreSqlConnection {get; set;} = string.Empty;
          public string MongoDbConnection {get; set;} = string.Empty;
          public string MongoDbDatabaseName {get; set;} = string.Empty;
          public int CommandTimeout {get; set;} = 30;
          public bool EnableSensitiveDataLogging { get; set; } = false;
          public bool EnableDetailedErrors {get; set;} = false;
}

using System;

namespace Bibliotech.Core.Models;

public class UserDNAProfile
{
          public string Id { get; set; } = string.Empty;
          public string UserId { get; set; } = string.Empty;
          public Dictionary<string, double> GenreAffinities { get; set; } = new();
          public Dictionary<string, double> AuthorAffinities { get; set; } = new();
          public Dictionary<string, double> ThemeAffinities { get; set; } = new();
          public ReadingPattern ReadingPattern { get; set; } = new();
          public DateTime LastEvolved { get; set; }
          public int Version { get; set; } = 1;
}

public class ReadingPattern
{
          public int AverageSessionDuration { get; set; }
          public List<string> PreferredReadingTimes {get; set;} = new();
          public double ReadingSpeed {get; set;}
          public double CompletionRate {get; set;}
          public double GenreJumpFrequency {get; set;}
}

using System;

namespace Bibliotech.Core.Models;

public class ReadingSession
{
          public string Id {get; set;} = string.Empty;
          public string UserId {get; set;} = string.Empty;
          public string BookId {get; set;} = string.Empty;
          public DateTime StartTime {get; set;}
          public DateTime EndTime {get; set;}
          public int Duration {get; set;}
          public int PagesRead {get; set;}
          public int CurrentPage {get; set;}
          public double EngagementScore {get; set;}
          public double ReadingSpeed {get; set;}
          public DeviceInfo DeviceInfo {get; set;} = new();
          public LocationInfo? Location {get; set;}
          public Dictionary<string, object> Metadata {get; set;} = new();
          public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class DeviceInfo
{
          public string Platform {get; set;} = string.Empty;
          public string AppVersion {get; set;} = string.Empty;
          public int ScreenTime {get; set;}
}

public class LocationInfo
{
          public double Latitude {get; set;}
          public double Longitude {get; set;}
          public double Accuracy {get; set;}
}

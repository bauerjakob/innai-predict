namespace InnAiServer.Options;

public class ApiClients
{
    public InnLevelClientOptions InnLevelClient { get; set; }
    public PrecipitationMapClientOptions PrecipitationMapClient { get; set; }
    public InnAiPredictionClientOptions InnAiPredictinoClient { get; set; }
}

public class InnLevelClientOptions
{
    public string BaseUrl { get; set; }
    public string[] Stations { get; set; }
}

public class PrecipitationMapClientOptions
{
    public string BaseUrl { get; set; }
    public int Zoom { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public string ColorPalette { get; set; }
    public string ApiKey { get; set; }
}

public class InnAiPredictionClientOptions
{
    public string BaseUrl { get; set; }
}
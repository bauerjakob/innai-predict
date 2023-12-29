using InnAi.Server.Extensions;
using InnAiServer.Options;
using Microsoft.Extensions.Options;

namespace InnAi.Server.Clients;

public class PrecipitationMapClient : IPrecipitationMapClient
{
    private readonly PrecipitationMapClientOptions _options;
    private readonly HttpClient _client;

    public PrecipitationMapClient(IOptions<ApiClients> options)
    {
        _options = options.Value.PrecipitationMapClient;
        
        _client = new HttpClient
        {
            BaseAddress = new Uri(_options.BaseUrl?.TrimEnd('/') ?? throw new Exception())
        };
    }
    
    public async Task<byte[]> GetAsync(DateTime dateTime)
    {
        var time = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);

        var url = GetPrecipitationUrl(dateTime);
        
        var result = await _client.GetAsync(url);
        result.EnsureSuccessStatusCode();
        var data = await result.Content.ReadAsByteArrayAsync();

        return data;
    }
    
    private string GetPrecipitationUrl(DateTime dateTime) =>
        $"/maps/2.0/weather/PAR0/{_options.Zoom}/{_options.X}/{_options.Y}?fill_bound=false&opacity=1&appid={_options.ApiKey}" +
        $"&palette={_options.ColorPalette}&date={dateTime.ToUnixTimeStamp()}";
}